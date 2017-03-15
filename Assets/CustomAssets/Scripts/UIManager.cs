using RSG;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	private Canvas canvas;
	private Image flash;
	private readonly PromiseTimer timedPromise = new PromiseTimer();

	public static UIManager Instance {
		get {
			if (instance == null) {
				instance = new GameObject("^UI").AddComponent<UIManager>();
			}
			return instance;
		}
	}
	private static UIManager instance;

	public void Awake() {
		if (instance == null) {
			instance = this;
			canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.sortingOrder = 100;
			canvas.worldCamera = Camera.main;
			flash = canvas.gameObject.AddComponent<Image>();
			flash.enabled = false;
		}
		else {
			Destroy(gameObject);
		}
	}

	public void Flash() {
		flash.enabled = true;
		timedPromise.WaitFor(0.1f).Done(() => flash.enabled = false);
	}

	public void Update() {
		timedPromise.Update(Time.deltaTime);
	}
}
