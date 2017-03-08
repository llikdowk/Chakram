using UnityEngine;

public class TouchHandler : MonoBehaviour {
#if UNITY_EDITOR
	private Touch fakeTouch;
#endif

	public void Update() {
	#if UNITY_EDITOR
		UnityEditorUpdate();
	#endif
	}

	private Vector3 lastMousePosition;
	public void OnGUI() {
	#if UNITY_EDITOR
		UnityEditorOnGUI();
	#endif
		
		foreach (var touch in Input.touches) {
			HandleTouch(touch);
		}

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
			HandleTouch(fakeTouch);
		}
	}

	private static void HandleTouch(Touch touch) {
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
}
