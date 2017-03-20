
using Game.Background;
using UnityEngine;

namespace Game {
	public static class GameState {
		private static readonly GameObject start = GameObject.Find("$start");
		public static void GameOver() {
			UIManager.Instance.Flash();
			score = 0;
			var playerTransform = Player.Instance.transform;
			playerTransform.parent = start.transform;
			playerTransform.localPosition = Vector3.zero;
			playerTransform.parent = null;
			BackgroundGenerator.Reset();
			BackgroundColorManager.Instance.SetNormalScoreMode();
			UIManager.Instance.Reset();
		}

		public static int Score {
			get { return score; }
			set {
				score = value;
				UIManager.Instance.SetScore(score);
			}
		}

		private static int score;
	}
}
