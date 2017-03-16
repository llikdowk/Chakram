
using UnityEngine;

namespace Game {
	public class Player : MonoBehaviour {
		private new CircleCollider2D collider;
		private int layerMaskAllButPlayer;
		private int lineLayer;
		public float Speed = 12.5f;
		public float RailSpeed = 17.5f;
		public static Player Instance;
		private Vector3 inheritedDirection = Vector3.up;

		public void Awake() {
			if (Instance == null) {
				Instance = this;
				UIManager ui = UIManager.Instance;
				collider = GetComponent<CircleCollider2D>();
				layerMaskAllButPlayer = ~(1 << LayerMaskManager.Get(Layer.Player));
				lineLayer = LayerMaskManager.Get(Layer.Line);
			}
			else {
				Destroy(gameObject);
			}
		}

		private const float inheritedHorizontalMultiplier = 3.0f;
		private const float inheritedLag = 8.5f;
		public void Update() {
			if (!CheckCollision(Physics2D.OverlapCircle(transform.position, collider.radius * transform.localScale.x,
					layerMaskAllButPlayer))) {
				transform.position += Vector3.up * Speed * Time.deltaTime + inheritedDirection * Time.deltaTime + Vector3.Project(inheritedDirection, Vector3.right) * inheritedHorizontalMultiplier * Time.deltaTime;
				inheritedDirection = Vector3.Lerp(inheritedDirection, Vector3.zero, Time.deltaTime * inheritedLag);
			}

		}

		private bool CheckCollision(Collider2D hitCollider) {
			if (!hitCollider) return false;

			if (hitCollider.gameObject.layer == lineLayer) {
				Line line = hitCollider.gameObject.GetComponent<Line>();
				Vector3 p = line.Vertices[0];
				Vector3 q = line.Vertices[1];
				Vector3 d = q - p;//transform.position;
				Vector2 dplayer = transform.position - p;
				Vector3 v = d.normalized;
				//Debug.Log(d.sqrMagnitude);
				if (d.sqrMagnitude < 0.1f) {
					return false;
				}
				inheritedDirection = d;
				gameObject.transform.position += v * Time.deltaTime * RailSpeed;// + (Vector3)(dplayer);
				return true;
			}
			else {
				GameState.GameOver();
				return false;
			}
		}
	}
}
