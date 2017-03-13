
using UnityEngine;

namespace CustomCamera {
	public class FollowPlayer : MonoBehaviour {
		public Transform Player;
		public float OffsetY = 4.8f;

		public void Update() {
			Vector3 p = Player.transform.position;
			transform.position = new Vector3(transform.position.x, p.y + OffsetY, transform.position.z);
		}
	}
}
