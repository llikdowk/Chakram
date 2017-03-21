﻿using UnityEngine;

namespace Visual {
	public class CircleWave : MonoBehaviour {
		public int NumCircles = 2;
		public Color Color = Color.white;
		private MeshRenderer renderer;
		private float toffset = 0.0f;

		public void Awake() {
			var matCircle = new Material(Shader.Find("Custom/Circle"));
			renderer = gameObject.GetComponent<MeshRenderer>();
			var circles = new Material[NumCircles];
			for(int i = 0; i < circles.Length; ++i) { 
				circles[i] = Object.Instantiate(matCircle);
				circles[i].color = Color;
			}
			renderer.materials = circles;
			toffset = Time.time - (int) Time.time;
		}

		public void Update() {
			float t = Time.time - toffset;
			for (int i = 0; i < renderer.materials.Length; ++i) {
				float taux = t + i * 0.25f;
				float tfraction = taux - (int) taux; 
				renderer.materials[i].SetFloat("_Radius", 0.5f * tfraction);
			}
		}

		public void OnEnable() {
			renderer.enabled = true;
			toffset = Time.time - (int) Time.time;
		}

		public void OnDisable() {
			renderer.enabled = false;
		}

	}
}
