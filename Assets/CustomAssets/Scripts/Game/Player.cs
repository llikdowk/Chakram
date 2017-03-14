
using UnityEngine;

namespace Game {
	public class Player : MonoBehaviour {
		private new CircleCollider2D collider;
		private int layerMaskAllButPlayer;
		private int lineLayer;
		public float Speed = 10.0f;
		public float RailSpeed = 15.0f;
		public static Player Instance;

		public void Awake() {
			if (Instance == null) {
				Instance = this;
				collider = GetComponent<CircleCollider2D>();
				layerMaskAllButPlayer = ~(1 << LayerMaskManager.Get(Layer.Player));
				lineLayer = LayerMaskManager.Get(Layer.Line);
			}
			else {
				Destroy(gameObject);
			}
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
				Vector3 d = q - p;//transform.position;
				Vector3 v = d.normalized;
				//Debug.Log(d.sqrMagnitude);
				if (d.sqrMagnitude < 0.1f) {
					return false;
				}
				gameObject.transform.position += v * Time.deltaTime * RailSpeed;
				return true;
			}
			else {
				GameState.GameOver();
				return false;
			}
		}
	}
}
