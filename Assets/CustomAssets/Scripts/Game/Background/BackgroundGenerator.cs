
using C5;
using UnityEngine;

namespace Game.Background {
	public class BackgroundGenerator : MonoBehaviour {
		private readonly C5.ArrayList<Transform> bgBlockPrefabs = new C5.ArrayList<Transform>();
		private readonly C5.ArrayList<Transform> fgBlockPrefabs = new C5.ArrayList<Transform>();
		private readonly C5.CircularQueue<Transform> instancedPrefabs = new C5.CircularQueue<Transform>(4);
		public static int generatedCount = 0;
		private const float offsetY = 19.2f;
		private readonly C5.C5Random randomizer = new C5Random();

		private void GenerateBlock() {
			Transform block;
			block = new GameObject("^block" + generatedCount).transform;
			Transform bg = Object.Instantiate(bgBlockPrefabs[randomizer.Next(0, bgBlockPrefabs.Count)]);
			bg.parent = block;
			bg.localPosition = Vector3.zero;
			Transform fg = Object.Instantiate(fgBlockPrefabs[randomizer.Next(0, fgBlockPrefabs.Count)]);
			fg.parent = block;
			fg.localPosition = Vector3.zero;

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
			Transform[] bgBlocks = Resources.LoadAll<Transform>("Prefabs/blocks/backgrounds");
			foreach (var b in bgBlocks) {
				bgBlockPrefabs.Add(b);
			}
			Transform[] fgBlocks = Resources.LoadAll<Transform>("Prefabs/blocks/foregrounds");
			foreach (var b in fgBlocks) {
				fgBlockPrefabs.Add(b);
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
