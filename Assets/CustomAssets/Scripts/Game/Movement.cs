
using UnityEngine;

namespace Game {
	public class Movement : MonoBehaviour {
		private const float Speed = 2.5f;

		public void Update() {
			transform.position += Vector3.down * Speed * Time.deltaTime;
		}
	}
}
