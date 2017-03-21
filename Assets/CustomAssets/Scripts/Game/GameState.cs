
using Game.Background;
using UnityEngine;

namespace Game {
	public static class GameState {
		private static readonly GameObject start = GameObject.Find("$start");
		public static void GameOver() {
			TouchHandler.Clear();
			UIManager.Instance.TutorialRestart();
			UIManager.Instance.Flash();
			++PlayerStatistics.Instance.Crashes;
			if (score > maxScore) {
				maxScore = score;
				PlayerStatistics.Instance.HighScore = maxScore;
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
			Player.Instance.start = true;
			PlayerStatistics.Instance.Save();
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

		private static int maxScore = PlayerStatistics.Instance.HighScore;
		private static int score;
		private static bool isInHighscoreMode;
	}
}
