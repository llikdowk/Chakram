
using UnityEngine;

namespace CustomCamera {
	public class FollowPlayer : MonoBehaviour {
		public Transform Player;
		private float offsetY;

		public void Awake() {
			offsetY = transform.position.y - GameObject.Find("_player").transform.position.y;

		}

		public void Update() {
			Vector3 p = Player.transform.position;
			transform.position = new Vector3(transform.position.x, p.y + offsetY, transform.position.z);
		}
	}
}
