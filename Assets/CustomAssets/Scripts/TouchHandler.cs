using System;
using RSG;
using UnityEngine;
using UnityEngine.Rendering;

public class TouchHandler : MonoBehaviour {
	private EdgeCollider2D edgeCollider;
	private LineRenderer lineRenderer;
	private readonly Vector3[] line = new Vector3[2];
	private readonly Vector2[] line2D = new Vector2[2];
	private Touch touch = new Touch();

	public void Awake() {
		edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.receiveShadows = false;
		lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
		lineRenderer.widthMultiplier = 0.10f;
	}

	private bool RefreshTouch() {
	#if UNITY_EDITOR
		if (TouchMockup.touches.Length == 0) {
			return false;
		}
		touch = TouchMockup.touches[0];
		return true;
	#else
		if (Input.touches.Length == 0) {
			return false;
		}
		touch = Input.touches[0];
		return true;
	#endif
	}

	public void Update() {
		if (RefreshTouch()) {
			HandleTouch();
		}
	}

	public void OnGUI() {
		if (RefreshTouch()) {
			ShowTouchInfo();
		}
	}

	private void ShowTouchInfo() {
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

	private void HandleTouch() {

		if (touch.phase == TouchPhase.Began) {
			line[0] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
			line[1] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
		} 
		else if (touch.phase == TouchPhase.Moved) {
			line[1] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
		}
		/* 
		else if (touch.phase == TouchPhase.Ended) {
			touchPromise.Resolve();
		} else if (touch.phase == TouchPhase.Canceled) {
			touchPromise.Reject(new Exception("Touch cancelled"));
		}
		*/
		lineRenderer.SetPositions(line);
		line2D[0] = line[0];
		line2D[1] = line[1];
		edgeCollider.points = line2D;

		//return touchPromise;
	}

	private void HandleNoTouch() {
		Debug.Log("HandleNoTouch");
	}
}
