
using UnityEngine;

namespace Game {
	public class Player : MonoBehaviour {
		private new CircleCollider2D collider;
		private int layerMaskAllButPlayer;

		public void Awake() {
			collider = GetComponent<CircleCollider2D>();
			layerMaskAllButPlayer = ~(1 << LayerMaskManager.Get(Layer.Player));
		}

		public void Update() {
			CheckCollision(Physics2D.OverlapCircle(transform.position, collider.radius, layerMaskAllButPlayer));
			
		}

		private static void CheckCollision(Collider2D hitCollider) {
			if (!hitCollider) return;
			Debug.Log(hitCollider);
		}
	}
}
