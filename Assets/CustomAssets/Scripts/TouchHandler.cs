using UnityEngine;
using UnityEngine.Rendering;

public class TouchHandler : MonoBehaviour {
#if UNITY_EDITOR
	private Touch fakeTouch;
	private Vector3 lastMousePosition;
#endif
	private EdgeCollider2D edgeCollider;
	private LineRenderer lineRenderer;
	private readonly Vector3[] line = new Vector3[2];
	private readonly Vector2[] line2D = new Vector2[2];

	public void Awake() {
		edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.receiveShadows = false;
		lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
		lineRenderer.widthMultiplier = 0.10f;
		//lineRenderer.startWidth = 0.215f;
		//lineRenderer.endWidth   = 0.100f;
	}

	public void Update() {
	#if UNITY_EDITOR
		UnityEditorUpdate();
	#endif
	}

	public void OnGUI() {
	#if UNITY_EDITOR
		UnityEditorOnGUI();
		HandleTouch(fakeTouch);
	#endif
		
		foreach (var touch in Input.touches) {
			ShowTouchInfo(touch);
			HandleTouch(touch);
		}

		/*
		if (Input.touches.Length == 0) {
			HandleNoTouch();
		}
		*/

	}

	private void UnityEditorUpdate() {
		if (Input.GetMouseButton(0)) {
			fakeTouch.fingerId = 0;
			fakeTouch.position = Input.mousePosition;
			fakeTouch.deltaTime = Time.deltaTime;
			fakeTouch.deltaPosition = Input.mousePosition - lastMousePosition;
			fakeTouch.phase = Input.GetMouseButtonDown(0)
				? TouchPhase.Began
				: (fakeTouch.deltaPosition.sqrMagnitude > 0f ? TouchPhase.Moved : TouchPhase.Stationary);
			fakeTouch.tapCount = 1;
			lastMousePosition = Input.mousePosition;
		}
	}

	private void UnityEditorOnGUI() {
		if (Input.GetMouseButton(0)) {
			ShowTouchInfo(fakeTouch);
		}
	}

	private static void ShowTouchInfo(Touch touch) {
		var message = "";
		message += "ID: " + touch.fingerId + "\n";
		message += "Phase: " + touch.phase + "\n";
		message += "TapCount: " + touch.tapCount + "\n";
		message += "deltaTime: " + touch.deltaTime + "\n";
		message += "deltaPosition: " + touch.deltaPosition + "\n";
		message += "Pos X: " + touch.position.x + "\n";
		message += "Pos Y: " + touch.position.y + "\n";

		int num = touch.fingerId;
		GUI.Label(new Rect(0 + 180 * num, 0, 170, 150), message);
	}

	private void HandleTouch(Touch touch) {
		lineRenderer.enabled = true;
		if (touch.phase == TouchPhase.Began) {
			line[0] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
			line[1] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
		} else if (touch.phase == TouchPhase.Moved) {
			line[1] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
		}
		lineRenderer.SetPositions(line);

		line2D[0] = line[0];
		line2D[1] = line[1];
		edgeCollider.points = line2D;
	}

	private void HandleNoTouch() {
		lineRenderer.enabled = false;
	}
}
