
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
	public int currentIndex = 0;
	private Material[] targetMaterials;
	private readonly PromiseTimer promiseTimer = new PromiseTimer();

	public void Awake() {
		colors[0] = ColorTuple1;
		colors[1] = ColorTuple2;
		colors[2] = ColorTuple3;
		colors[3] = ColorTuple4;

		Shader bgShader = Shader.Find("Custom/UnlitTextureColor");
		targetMaterials = transform.GetComponentsInChildren<MeshRenderer>()
			.Where(r => r.material.shader == bgShader)
			.Select(r => r.material).ToArray();
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
		const float durationSeconds = 2.0f;
		var colorSequence = colors[currentIndex].Select(c => (Func<IPromise>)(() => LerpColor(c, durationSeconds)));
		return Promise.Sequence(colorSequence).Then(()=>ColorCycle());
	}

	public void Start() {
		ColorCycle().Catch(Debug.LogError);
	}

	public void Update() {
		promiseTimer.Update(Time.deltaTime);
	}

}
