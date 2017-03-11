
using UnityEngine;

namespace Game {
	public class Player : MonoBehaviour {
		private new CircleCollider2D collider;
		private int layerMaskAllButPlayer;
		private int lineLayer;

		public void Awake() {
			collider = GetComponent<CircleCollider2D>();
			layerMaskAllButPlayer = ~(1 << LayerMaskManager.Get(Layer.Player));
			lineLayer = LayerMaskManager.Get(Layer.Line);
		}

		public void Update() {
			CheckCollision(Physics2D.OverlapCircle(transform.position, collider.radius * transform.localScale.x, layerMaskAllButPlayer));
			
		}

		//private bool first = true;
		private void CheckCollision(Collider2D hitCollider) {
			if (!hitCollider) return;
			if (hitCollider.gameObject.layer == lineLayer) {
				Line line = hitCollider.gameObject.GetComponent<Line>();
				Vector3 p = line.Vertices[0];
				Vector3 q = line.Vertices[1];
				Vector3 v = (q - p).normalized;

				/*
				if (first) {
					Vector3 c = gameObject.transform.position;
					float lx = (c.x - p.x) / (v.x - 0.0f);
					float ly = (c.y - p.y) / (v.y - 1.0f);
					Vector2 magnetPoint = new Vector2(p.x + lx * v.x, p.y + ly * v.y);
					gameObject.transform.position = magnetPoint;
					first = false;
				}
				*/
				gameObject.transform.position += v * Time.deltaTime * 9.0f;

			}
			else {
				Debug.Log("game over");
			}

		}
	}
}
