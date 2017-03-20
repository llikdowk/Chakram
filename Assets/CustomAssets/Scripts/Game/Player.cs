
using Game.Background;
using UnityEngine;

namespace Game {
	public class Player : MonoBehaviour {
		public float Speed = 12.5f;
		public float RailSpeed = 17.5f;
		public float InheritedHorizontalMultiplier = 10.0f;
		public float InheritedToZeroSpeed = 8.5f;

		private new CircleCollider2D collider;
		private int layerMaskAllButPlayer;
		private int lineLayer;
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
		
		private readonly Collider2D[] collisionsFound = new Collider2D[2];
		public void Update() {
			int ncol = Physics2D.OverlapCircleNonAlloc(transform.position, collider.radius * transform.localScale.x, collisionsFound,
				layerMaskAllButPlayer);
			
			if (!CheckCollisions(ncol)) {
				transform.position += Vector3.up * Speed * Time.deltaTime + inheritedDirection * Time.deltaTime + Vector3.Project(inheritedDirection, Vector3.right) * InheritedHorizontalMultiplier * Time.deltaTime;
				inheritedDirection = Vector3.Lerp(inheritedDirection, Vector3.zero, Time.deltaTime * InheritedToZeroSpeed);
			}

		}

		private bool CheckCollisions(int ncol) {
			if (ncol == 0) return false;

			for (int i = 0; i < ncol; ++i) {
				Collider2D hitCollider = collisionsFound[i];
				if (hitCollider.isTrigger) {
					if (hitCollider.gameObject.layer == LayerMaskManager.Get(Layer.Start)) {
						BackgroundGenerator.PlayerInSafeZone = false;
					}
				}
				else if (hitCollider.gameObject.layer == lineLayer) {
					Line line = hitCollider.gameObject.GetComponent<Line>();
					Vector3 p = line.Vertices[0];
					Vector3 q = line.Vertices[1];
					Vector3 d = q - transform.position; // - p
					Vector3 v = d.normalized;
					//Debug.Log(d.sqrMagnitude);

					inheritedDirection = (q - p).normalized;
					if (d.sqrMagnitude < 1.0f) {
						return false;
					}
					gameObject.transform.position += v * Time.deltaTime * RailSpeed; // + (Vector3)(dplayer);
					return true;
				}
				else {
					GameState.GameOver();
					return false;
				}
			}
			return false;
		}
	}
}
