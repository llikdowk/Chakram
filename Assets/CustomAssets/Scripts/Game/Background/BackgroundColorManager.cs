
using System;
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
	private readonly C5.ArrayList<SpriteRenderer> targetMaterials = new C5.ArrayList<SpriteRenderer>(6);
	private readonly PromiseTimer promiseTimer = new PromiseTimer();

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

	public static void Register(SpriteRenderer[] renderers) {
		foreach (var r in renderers) {
			instance.targetMaterials.Add(r);
		}
	}

	public static void Unregister(SpriteRenderer[] renderers) {
		foreach (var r in renderers) {
			instance.targetMaterials.Remove(r);
		}
	}

	private IPromise LerpColor(Color endColor, float durationSeconds) {
		var startColor = targetMaterials.First().color;
		return promiseTimer.WaitUntil(timeData => {
			float t = timeData.elapsedTime / durationSeconds;
			foreach (var material in targetMaterials) {
				material.color = Color.Lerp(startColor, endColor, t);
			}
			return t >= 1.0f;
		});
	}

	private IPromise ColorCycle() {
		var colorSequence = colors[CurrentIndex].Select(c => (Func<IPromise>)(() => LerpColor(c, DurationSeconds)));
		return Promise.Sequence(colorSequence).Then(()=>ColorCycle());
	}

	public void Start() {
		ColorCycle().Catch(Debug.LogError);
	}

	public void Update() {
		promiseTimer.Update(Time.deltaTime);
	}

}
