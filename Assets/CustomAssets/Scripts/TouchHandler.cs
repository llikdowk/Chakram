using Game;
using UnityEngine;
using UnityEngine.Rendering;

public class TouchHandler : MonoBehaviour {
	private LineRenderer line;
	private readonly Vector3[] lineVertices3D = new Vector3[2];
	private Touch touch;

	public void Awake() {
		line = gameObject.AddComponent<LineRenderer>();
		line.receiveShadows = false;
		line.shadowCastingMode = ShadowCastingMode.Off;
		line.widthMultiplier = 0.10f;
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


	public void Start() {
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
			line.enabled = true;
			lineVertices3D[0] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
			lineVertices3D[1] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
		} 
		else if (touch.phase == TouchPhase.Moved) {
			lineVertices3D[1] = Camera.main.ScreenToWorldPoint(touch.position) + Vector3.forward;
		}
		else if (touch.phase == TouchPhase.Ended) {
			OnUpTouch();
		}
		else if (touch.phase == TouchPhase.Canceled) {
			OnUpTouch();
		}
		
		line.SetPositions(lineVertices3D);
	}

	private void OnUpTouch() {
		Debug.Log("OnUpTouch called");
		
		GameObject g = new GameObject("^line");
		var sceneLine = g.AddComponent<Line>();
		sceneLine.Create(line);
		sceneLine.MoveWithScene = true;
		g.AddComponent<Movement>();
		line.enabled = false;
	}
}
