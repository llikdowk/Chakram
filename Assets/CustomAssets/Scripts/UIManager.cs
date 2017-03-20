using System;
using CustomDebug;
using RSG;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	private Canvas canvas;
	private Image flash;
	private Text score;
	private Text maxScore;
	private readonly PromiseTimer timedPromise = new PromiseTimer();

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

		}
		else {
			Destroy(gameObject);
		}
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
