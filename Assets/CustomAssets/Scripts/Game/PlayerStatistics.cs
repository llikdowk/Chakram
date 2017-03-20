
using UnityEngine;

namespace Game {
	public class PlayerStatistics : IPersistant {

		public int HighScore = -1;
		private readonly string[] medals = new string[5];
		public int NextMedal = 0;

		public static PlayerStatistics Instance {
			get {
				if (instance == null) instance = new PlayerStatistics();
				return instance;
			}
		}
		private static PlayerStatistics instance;

		private PlayerStatistics() {
			medals[0] = "Next medal: Bronze medal, at 10";
			medals[1] = "Next medal: Silver medal, at 25";
			medals[2] = "Next medal: Gold medal, at 50";
			medals[3] = "Next medal: Platinum medal, at 100";
			medals[4] = "You've reached the max medal! but have no doubt that someone in the world is better than you. Always.";
			Load();
		}

		public void Load() {
			NextMedal = PlayerPrefs.GetInt("NextMedal");
			HighScore = PlayerPrefs.GetInt("HighScore");
		}

		public void Save() {
			PlayerPrefs.SetInt("NextMedal", NextMedal);
			PlayerPrefs.SetInt("HighScore", HighScore);
		}

		public void Clear() {
			PlayerPrefs.SetInt("NextMedal", 0);
			PlayerPrefs.SetInt("HighScore", -1);
		}

	}
}
