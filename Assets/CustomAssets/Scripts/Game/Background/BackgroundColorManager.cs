
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RSG;

public class BackgroundColorManager : MonoBehaviour {
	public Color[] ColorTuple1;
	public Color[] ColorTuple2;
	public Color[] ColorTuple3;
	public Color[] ColorTuple4;

	private readonly Color[][] colors = new Color[4][];
	public int CurrentIndex = 0;
	public float DurationSeconds = 2.0f;

	private class SceneSprite {
		public readonly C5.ArrayList<SpriteRenderer> Renderers = new C5.ArrayList<SpriteRenderer>();
		public bool Destroyed;

		public void AddRenderers(SpriteRenderer[] renderers) {
			foreach (var r in renderers) {
				Renderers.Add(r);
			}
		}
	}
	private readonly C5.ArrayList<SceneSprite> targetBlocks = new C5.ArrayList<SceneSprite>(6);
	private readonly PromiseTimer promiseTimer = new PromiseTimer();
	private readonly Dictionary<int, SceneSprite> map = new Dictionary<int, SceneSprite>();

	private static BackgroundColorManager instance;

	public void Awake() {
		if (instance == null) {
			instance = this;
			colors[0] = ColorTuple1;
			colors[1] = ColorTuple2;
			colors[2] = ColorTuple3;
			colors[3] = ColorTuple4;
		}
		else {
			Destroy(gameObject);
		}
	}

	public static void Register(GameObject block) {
		var sceneSprite = new SceneSprite();
		instance.map.Add(block.GetInstanceID(), sceneSprite);
		sceneSprite.AddRenderers(block.GetComponentsInChildren<SpriteRenderer>());
		instance.targetBlocks.Add(sceneSprite);
	}

	public static void Unregister(GameObject block) {
		instance.map[block.GetInstanceID()].Destroyed = true;
	}

	private IPromise LerpColor(Color endColor, float durationSeconds) {
		//var startColor = targetBlocks.First().Renderers.First.color;
		return promiseTimer.WaitUntil(timeData => {
			float t = timeData.elapsedTime / durationSeconds;
			foreach (var block in targetBlocks) {
				var startColor = block.Renderers.First.color;
				foreach (var r in block.Renderers) {
					r.material.color = Color.Lerp(startColor, endColor, t);
				}
			}
			return t >= 1.0f;
		});
	}

	private IPromise ColorCycle() {
		var colorSequence = colors[CurrentIndex].Select(c => (Func<IPromise>)(() => LerpColor(c, DurationSeconds)));
		return Promise.Sequence(colorSequence).
			Then(() => {
				foreach (var block in targetBlocks) {
					if (block.Destroyed) {
						targetBlocks.Remove(block);
					}
				}
			}).Then(()=>ColorCycle());
	}

	public void Start() {
		ColorCycle().Catch(Debug.LogError);
	}

	public void Update() {
		promiseTimer.Update(Time.deltaTime);
	}

}
