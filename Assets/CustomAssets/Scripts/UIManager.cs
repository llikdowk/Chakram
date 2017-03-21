using CustomDebug;
using RSG;
using UnityEngine;
using UnityEngine.UI;
using Visual;

public class UIManager : MonoBehaviour {
	private Canvas canvas;
	private Image flash;
	private Text score;
	private Text maxScore;
	private readonly PromiseTimer timedPromise = new PromiseTimer();
	private GameObject circleWavesStart;
	private GameObject circleWavesEnd;

	public static UIManager Instance {
		get {
			if (instance == null) {
				var ui = GameObject.Find("$UI");
				if (!ui) {
					DebugMsg.GameObjectNotFound(Debug.LogError, "$UI");
				}
				else {
					instance = ui.AddComponent<UIManager>();
				}
			}
			return instance;
		}
	}
	private static UIManager instance;

	public void Awake() {
		if (instance == null) {
			instance = this;
			canvas = gameObject.GetComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.sortingOrder = 100;
			canvas.worldCamera = Camera.main;
			flash = canvas.gameObject.GetComponent<Image>();
			flash.enabled = false;
			score = GameObject.Find("$score").transform.GetComponentInChildren<Text>();
			maxScore = GameObject.Find("$maxScore").transform.GetComponentInChildren<Text>();
			circleWavesStart = GameObject.Find("$CircleWavesStart");
			if (!circleWavesStart) {
				DebugMsg.GameObjectNotFound(Debug.LogWarning, "$CircleWavesStart");
			}
			else {
				circleWavesStart.SetActive(true);
			}
			circleWavesEnd = GameObject.Find("$CircleWavesEnd");
			if (!circleWavesEnd) {
				DebugMsg.GameObjectNotFound(Debug.LogWarning, "$CircleWavesEnd");
			}
			else {
				circleWavesEnd.SetActive(true);
				circleWavesEnd.GetComponent<CircleWave>().enabled = false;
			}

		}
		else {
			Destroy(gameObject);
		}
	}

	public void TutorialShowEnd() {
		circleWavesStart.GetComponent<CircleWave>().enabled = false;
		circleWavesEnd.GetComponent<CircleWave>().enabled = true;
	}

	public void TutorialRestart() {
		circleWavesStart.SetActive(true);
		circleWavesEnd.SetActive(true);
		circleWavesStart.GetComponent<CircleWave>().enabled = true;
		circleWavesEnd.GetComponent<CircleWave>().enabled = false;
	}

	public void TutorialHide() {
		circleWavesStart.SetActive(false);
		circleWavesEnd.SetActive(false);
	}

	public void Flash() {
		flash.enabled = true;
		timedPromise.WaitFor(0.1f).Done(() => flash.enabled = false);
	}

	public void SetScore(int scoreValue) {
		this.score.text = scoreValue.ToString();
	}

	public void SetMaxScore(int maxScoreValue) {
		maxScore.text = maxScoreValue.ToString();
	}

	public void Update() {
		timedPromise.Update(Time.deltaTime);
	}
}
