
using UnityEngine;
using UnityEngine.Rendering;

namespace Game {
	public class Line : MonoBehaviour {
		public LineRenderer Renderer;
		public EdgeCollider2D Collider;
		public bool MoveWithScene;
		private readonly Vector3[] empty = new Vector3[0];
		private readonly Vector3[] vertices = new Vector3[2];
		private readonly Vector2[] lineVertices2D = new Vector2[2];
		private Vector3 lastPosition;

		public void Create(LineRenderer other) {
			Collider = gameObject.AddComponent<EdgeCollider2D>();
			Renderer = gameObject.AddComponent<LineRenderer>();

			other.GetPositions(vertices);
			Renderer.SetPositions(vertices);
			Renderer.receiveShadows = false;
			Renderer.shadowCastingMode = ShadowCastingMode.Off;
			Renderer.widthMultiplier = 0.10f;

			lineVertices2D[0] = vertices[0];
			lineVertices2D[1] = vertices[1];
			Collider.points = lineVertices2D;
		}

		public void Update() {
			if (MoveWithScene) {
				Vector3 deltaPosition = transform.position - lastPosition;
				lastPosition = transform.position;

				Renderer.GetPositions(vertices);
				vertices[0] += deltaPosition;
				vertices[1] += deltaPosition;
				Renderer.SetPositions(vertices);

			}
			
		}

	}
}
