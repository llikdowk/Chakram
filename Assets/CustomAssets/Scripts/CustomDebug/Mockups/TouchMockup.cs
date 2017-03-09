
using UnityEngine;

public class TouchMockup : MonoBehaviour {

	public static Touch[] touches {
		get { return instance.hasTouch ? instance.withTouches : instance.noTouches; }
	}

	private readonly Touch[] noTouches = new Touch[0];
	private readonly Touch[] withTouches = new Touch[1];
	private Vector3 lastMousePosition;
	private Touch fakeTouch;
	private bool hasTouch;

	private static TouchMockup instance;

	public void Awake() {
		if (instance == null) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	public void Update() {
		if (Input.GetMouseButton(0)) {
			hasTouch = true;
			fakeTouch.fingerId = 0;
			fakeTouch.position = Input.mousePosition;
			fakeTouch.deltaTime = Time.deltaTime;
			fakeTouch.deltaPosition = Input.mousePosition - lastMousePosition;
			fakeTouch.phase = Input.GetMouseButtonDown(0)
				? TouchPhase.Began
				: (fakeTouch.deltaPosition.sqrMagnitude > 0f ? TouchPhase.Moved : TouchPhase.Stationary);
			fakeTouch.tapCount = 1;
			lastMousePosition = Input.mousePosition;
			hasTouch = true;
			withTouches[0] = fakeTouch;
		}
		else {
			if (Input.GetMouseButtonUp(0)) {
				withTouches[0].phase = TouchPhase.Ended;
			}
			else {
				hasTouch = false;
			}
		}

	}
}