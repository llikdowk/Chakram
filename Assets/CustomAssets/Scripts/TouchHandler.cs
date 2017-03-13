using CustomDebug;
using Game;
using UnityEngine;
using UnityEngine.Rendering;

public class TouchHandler : MonoBehaviour {
	private static Material material;
	private LineRenderer line;
	private readonly Vector3[] lineVertices3D = new Vector3[2];
	private Touch touch;

	public void Awake() {
		if (!material) {
			const string path = "Materials/mat_linePressed";
			material = Resources.Load<Material>(path);
			if (!material) {
				DebugMsg.ResourceNotFound(Debug.LogError, path);
			}
		}
		line = gameObject.AddComponent<LineRenderer>();
		line.receiveShadows = false;
		line.shadowCastingMode = ShadowCastingMode.Off;
		line.widthMultiplier = 0.10f;
		line.material = material;
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
			Vector3 p = Camera.main.ScreenToWorldPoint(touch.position);
			p = new Vector3(p.x, p.y, 1.0f);
			lineVertices3D[0] = p;
			lineVertices3D[1] = p;
		} 
		else if (touch.phase == TouchPhase.Moved) {
			Vector3 p = Camera.main.ScreenToWorldPoint(touch.position);
			p = new Vector3(p.x, p.y, 1.0f);
			lineVertices3D[1] = p;
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
		GameObject g = new GameObject("^line");
		var sceneLine = g.AddComponent<Line>();
		sceneLine.Create(line);
		sceneLine.MoveWithScene = true;
		line.enabled = false;
	}
}
