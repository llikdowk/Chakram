using CustomDebug;
using Game;
using UnityEngine;
using UnityEngine.Rendering;

public class TouchHandler : MonoBehaviour {
	private static Material material;
	private static LineRenderer line;
	private readonly Vector3[] lineVertices3D = new Vector3[2];
	private Touch touch;
	private static bool ignoreTouch;
	private static bool pressed = false;
	private static GameObject lineCreated;
	private static bool inTutorial = false;

	public static void Clear() {
		line.SetPositions(new Vector3[2] {Vector3.zero, Vector3.zero});
		if (lineCreated) {
			Object.Destroy(lineCreated);
		}
		if (pressed) {
			ignoreTouch = true;
		}
	}

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
		line.widthMultiplier = 0.50f;
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

	/*
	public void OnGUI() {
		if (RefreshTouch()) {
			ShowTouchInfo();
		}
	}
	*/

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
			pressed = true;
			line.enabled = true;
			Vector3 p = Camera.main.ScreenToWorldPoint(touch.position);
			Ray ray = new Ray(p, Vector3.forward);
			if (Physics.Raycast(ray, 100, 1 << LayerMaskManager.Get(Layer.UI))) {
				UIManager.Instance.TutorialShowEnd();
				inTutorial = true;
			}

			p = new Vector3(p.x, p.y, 1.0f);
			lineVertices3D[0] = p;
			lineVertices3D[1] = p;
			line.SetPositions(lineVertices3D);

		}
		else if (touch.phase == TouchPhase.Ended) {
			OnUpTouch();
		}
		else if (touch.phase == TouchPhase.Canceled) {
			OnUpTouch();
		}
		else {
			if (!ignoreTouch) {
				Vector3 p = Camera.main.ScreenToWorldPoint(touch.position);
				p = new Vector3(p.x, p.y, 1.0f);
				lineVertices3D[1] = p;
				line.SetPositions(lineVertices3D);
			}
		}
	}

	private void OnUpTouch() {
		if (ignoreTouch) {
			ignoreTouch = false;
			return;
		}
		pressed = false;

		if (inTutorial) {
			Vector3 p = Camera.main.ScreenToWorldPoint(touch.position);
			Ray ray = new Ray(p, Vector3.forward);
			if (Physics.Raycast(ray, 100, 1 << LayerMaskManager.Get(Layer.UI))) {
				UIManager.Instance.TutorialRestart();
			}
			else {
				UIManager.Instance.TutorialHide();
				inTutorial = false;
				OnUpTouch();
			}
		}
		else {
			GameObject g = new GameObject("^line");
			var sceneLine = g.AddComponent<Line>();
			sceneLine.Create(line);
			sceneLine.MoveWithScene = true;
			line.enabled = false;
			lineCreated = g;
		}
	}
}
