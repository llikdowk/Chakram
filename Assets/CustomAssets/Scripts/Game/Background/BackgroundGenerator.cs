
using UnityEngine;

namespace Game.Background {
	public class BackgroundGenerator : MonoBehaviour {
		private readonly C5.ArrayList<Transform> blockPrefabs = new C5.ArrayList<Transform>();
		private readonly C5.CircularQueue<Transform> instancedPrefabs = new C5.CircularQueue<Transform>(3);
		private int generatedCount = 0;
		private const float offsetY = 19.2f;

		private Transform GenerateBlock() {
			Transform block = Object.Instantiate(blockPrefabs.First); // TODO: get random
			block.transform.parent = transform;
			block.transform.position = Vector3.up * generatedCount * offsetY;
			//BackgroundColorManager.Register(block.GetComponentsInChildren<SpriteRenderer>());
			++generatedCount;
			instancedPrefabs.Enqueue(block);
			if (instancedPrefabs.Count == 3) {
				Transform old = instancedPrefabs.Dequeue();
				//BackgroundColorManager.Unregister(block.GetComponentsInChildren<SpriteRenderer>());
				Object.Destroy(old.gameObject);
			}
			return block;
		}

		public void Awake() {
			blockPrefabs.Add(Resources.Load<Transform>("Prefabs/block1"));
			GenerateBlock();
		}

		public void Update() {
			if (Camera.main.transform.position.y + offsetY > generatedCount * offsetY) {
				GenerateBlock();
			}
		}

	}
}
