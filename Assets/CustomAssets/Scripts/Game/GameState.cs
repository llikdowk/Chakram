
using Game.Background;
using UnityEngine;

namespace Game {
	public static class GameState {
		public static void GameOver() {
			Debug.Log("TODO: gameover stuff"); //TODO
			/*
			Player.Instance.transform.position = Vector3.zero;
			BackgroundGenerator.generatedCount = 0;
			*/
			UIManager.Instance.Flash();
			score = 0;
			GameObject start = GameObject.Find("$start");
			var playerTransform = Player.Instance.transform;
			playerTransform.parent = start.transform;
			playerTransform.localPosition = Vector3.zero;
			playerTransform.parent = null;
			BackgroundGenerator.Reset();
		}

		public static int Score {
			get { return score; }
			set {
				score = value;
				Debug.Log(">>score= " + score);
			}
		}

		private static int score;
	}
}
