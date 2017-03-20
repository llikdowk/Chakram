
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RSG;


public class BackgroundColorManager : MonoBehaviour {

	[System.Serializable]
	public struct ColorTuple {
		public Color[] Tuple;
	}

	public ColorTuple[] Colors;
	public Color[] ColorsRecordBreak;

	public Color[] CurrentPair;
	private Color forcedColor;
	public float DurationSeconds = 2.0f;
	private bool highScoreMode;
	private bool breakColorCycle;

	private readonly C5.IList<SpriteRenderer> targetBlocks = new C5.ArrayList<SpriteRenderer>(16);
	private readonly PromiseTimer promiseTimer = new PromiseTimer();
	private readonly Dictionary<int, C5.ArrayList<SpriteRenderer>> map = new Dictionary<int, C5.ArrayList<SpriteRenderer>>();
	public static BackgroundColorManager Instance;

	public void SetHighScoreMode() {
		CurrentPair = ColorsRecordBreak;
		forcedColor = CurrentPair[0];
		breakColorCycle = true;
		/*
		highScoreMode = true;
		lastPair = CurrentPair;
		CurrentPair = ColorTupleRecordBreak;
		*/
	}

	public void SetNormalScoreMode() {
		CurrentPair = Colors.First().Tuple;
		forcedColor = CurrentPair[0];
		breakColorCycle = true;
		/*
		CurrentPair = (++lastPair % 3);
		highScoreMode = false;
		*/
	}

	public void Awake() {
		if (Instance == null) {
			Instance = this;
			/*
			colors[0] = ColorTuple1;
			colors[1] = ColorTuple2;
			colors[2] = ColorTuple3;
			*/
		}
		else {
			Destroy(gameObject);
		}
	}

	public static void Register(GameObject block) {
		var childRenderers = block.GetComponentsInChildren<SpriteRenderer>();
		int count = 0;
		foreach(var renderer in childRenderers) {
			if (renderer.name == "securityZone") {
				continue;
			}
			Instance.targetBlocks.Add(renderer);
			++count;
		}
		Instance.map.Add(block.GetInstanceID(), 
			(C5.ArrayList<SpriteRenderer>) Instance.targetBlocks.View(
				Instance.targetBlocks.Count-count, 
				count
				));
	}

	public static void Unregister(GameObject block) {
		Instance.map[block.GetInstanceID()].Clear();
		Instance.map.Remove(block.GetInstanceID());
	}

	private IPromise LerpColor(Color endColor, float durationSeconds) {
		var startColor = targetBlocks.First.material.color;
		return promiseTimer.WaitUntil(timeData => {
			float t = timeData.elapsedTime / durationSeconds;
			foreach (var r in targetBlocks) {
				if (!breakColorCycle) {
					r.material.color = Color.Lerp(startColor, endColor, t);
				}
				else {
					r.material.color = forcedColor;
				}
			}
			return t >= 1.0f || breakColorCycle;
		});
	}

	private IPromise ColorCycle() {
		var colorSequence = CurrentPair
			.Select(c => (Func<IPromise>)(() => LerpColor(c, DurationSeconds)));
		return Promise.Sequence(colorSequence).Then(() => {
			if (breakColorCycle) {
				breakColorCycle = false;
			}
			ColorCycle();
		});
	}

	public void Start() {
		CurrentPair = Colors.First().Tuple;
		ColorCycle().Catch(Debug.LogError);
	}
	
	public void Update() {
		promiseTimer.Update(Time.deltaTime);
	}

}
