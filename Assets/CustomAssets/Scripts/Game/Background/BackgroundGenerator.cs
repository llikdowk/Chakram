
using C5;
using UnityEngine;

namespace Game.Background {
	public class BackgroundGenerator : MonoBehaviour {
		private readonly C5.ArrayList<Transform> blockPrefabs = new C5.ArrayList<Transform>();
		private readonly C5.CircularQueue<Transform> instancedPrefabs = new C5.CircularQueue<Transform>(4);
		private int generatedCount = 0;
		private const float offsetY = 19.2f;
		private readonly C5.C5Random randomizer = new C5Random();

		private void GenerateBlock() {
			Transform block = Object.Instantiate(blockPrefabs[randomizer.Next(0, blockPrefabs.Count)]);
			block.transform.parent = transform;
			block.transform.position = Vector3.up * generatedCount * offsetY;
			BackgroundColorManager.Register(block.gameObject);
			++generatedCount;
			instancedPrefabs.Enqueue(block);
			if (instancedPrefabs.Count == 4) {
				Transform old = instancedPrefabs.Dequeue();
				BackgroundColorManager.Unregister(old.gameObject);
				Object.Destroy(old.gameObject);
			}
		}

		public void Awake() {
			Transform[] blocks = Resources.LoadAll<Transform>("Prefabs/blocks");
			foreach (var b in blocks) {
				blockPrefabs.Add(b);
			}
			GenerateBlock();
		}

		public void FixedUpdate() {
			if (Camera.main.transform.position.y + offsetY*1.25f> generatedCount * offsetY) {
				GenerateBlock();
			}
		}

	}
}
