
using UnityEngine;

namespace Game {
	public class Player : MonoBehaviour {
		private new CircleCollider2D collider;
		private int layerMaskAllButPlayer;
		private int lineLayer;
		public float Speed = 4.5f;

		public void Awake() {
			collider = GetComponent<CircleCollider2D>();
			layerMaskAllButPlayer = ~(1 << LayerMaskManager.Get(Layer.Player));
			lineLayer = LayerMaskManager.Get(Layer.Line);
		}

		public void Update() {
			if (!CheckCollision(Physics2D.OverlapCircle(transform.position, collider.radius * transform.localScale.x,
					layerMaskAllButPlayer))) 
			{
				transform.position += Vector3.up * Speed * Time.deltaTime;
			}

		}

		private bool CheckCollision(Collider2D hitCollider) {
			if (!hitCollider) return false;

			if (hitCollider.gameObject.layer == lineLayer) {
				Line line = hitCollider.gameObject.GetComponent<Line>();
				Vector3 p = line.Vertices[0];
				Vector3 q = line.Vertices[1];
				Vector3 d = q - transform.position;
				Vector3 v = d.normalized;
				Debug.Log(d.sqrMagnitude);
				if (d.sqrMagnitude < 0.5f) {
					return false;
				}
				gameObject.transform.position += v * Time.deltaTime * 9.0f;
				return true;
			}
			else {
				GameState.GameOver();
				return false;
			}
		}
	}
}
