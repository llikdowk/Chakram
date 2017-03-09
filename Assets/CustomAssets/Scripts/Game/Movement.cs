
using UnityEngine;

namespace Game {
	public class Movement : MonoBehaviour {
		public float Speed = 1.0f;

		public void Update() {
			transform.position += Vector3.down * Speed * Time.deltaTime;
		}
	}
}
