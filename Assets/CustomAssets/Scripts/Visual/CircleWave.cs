using UnityEngine;

namespace Visual {
	public class CircleWave : MonoBehaviour {
		private Material matCircle;

		public void Awake() {
			//matCircle = new Material(Shader.Find("Custom/Circle"));
			//gameObject.GetComponent<Renderer>().material = matCircle;
			matCircle = gameObject.GetComponent<Renderer>().material;
		}


	}
}
