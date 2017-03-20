
using Assets.CustomAssets.Scripts.Game;
using Game.Background;
using UnityEngine;

namespace Game {
	public static class GameState {
		private static readonly GameObject start = GameObject.Find("$start");
		public static void GameOver() {
			UIManager.Instance.Flash();
			if (score > maxScore) {
				maxScore = score;
				PlayerStatistics.HighScore = maxScore;
				UIManager.Instance.SetMaxScore(maxScore);
			}
			score = 0;
			var playerTransform = Player.Instance.transform;
			playerTransform.parent = start.transform;
			playerTransform.localPosition = Vector3.zero;
			playerTransform.parent = null;
			BackgroundGenerator.Reset();
			BackgroundColorManager.Instance.SetNormalScoreMode();
			isInHighscoreMode = false;
			TouchHandler.Clear();
			Player.Instance.start = true;
		}

		public static int Score {
			get { return score; }
			set {
				score = value;
				UIManager.Instance.SetScore(score);
				if (!isInHighscoreMode && score > maxScore && maxScore > 1) {
					BackgroundColorManager.Instance.SetHighScoreMode();
					isInHighscoreMode = true;
				}
			}
		}

		private static int maxScore = -1;
		private static int score;
		private static bool isInHighscoreMode;
	}
}
