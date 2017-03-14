
using Game.Background;
using UnityEngine;

namespace Game {
	public static class GameState {
		public static void GameOver() {
			//Debug.Log("TODO: gameover stuff"); //TODO
			Player.Instance.transform.position = Vector3.zero;
			BackgroundGenerator.generatedCount = 0;
		}
	}
}
