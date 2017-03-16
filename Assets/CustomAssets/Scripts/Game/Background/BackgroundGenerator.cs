
using Assets.CustomAssets.Scripts.DataStructures;
using C5;
using UnityEngine;

namespace Game.Background {
	public class BackgroundGenerator : MonoBehaviour {
		private static readonly C5.CircularQueue<Transform> instancedPrefabs = new C5.CircularQueue<Transform>(4);
		private static int generatedCount = 0;
		private const float offsetY = 19.2f;
		private static MarkovChain<Transform> fgChain;
		private static MarkovChain<Transform> bgChain;

		private void GenerateBlock() {
			Transform block = new GameObject("^block" + generatedCount).transform;
			Transform bg = bgChain.Next();
			bg.parent = block;
			bg.localPosition = Vector3.zero;
			Transform fg = fgChain.Next();
			fg.parent = block;
			fg.localPosition = Vector3.zero;

			block.transform.parent = transform;
			block.transform.position = Vector3.up * generatedCount * offsetY;
			BackgroundColorManager.Register(block.gameObject);
			++generatedCount;
			GameState.Score = generatedCount;
			instancedPrefabs.Enqueue(block);
			if (instancedPrefabs.Count == 4) {
				Transform old = instancedPrefabs.Dequeue();
				BackgroundColorManager.Unregister(old.gameObject);
				Object.Destroy(old.gameObject);
			}
		}

		public static void Reset() {
			fgChain.Current = fgChain.Root;
			generatedCount = 0;
		}

		public void Awake() {
			fgChain = new MarkovChain<Transform>();
			bgChain = new MarkovChain<Transform>();

			Transform[] bgBlocks = Resources.LoadAll<Transform>("Prefabs/blocks/backgrounds");
			foreach (var b in bgBlocks) {
				bgChain.AddNode(new MarkovChain<Transform>.Node(b));
			}
			Transform[] fgBlocks = Resources.LoadAll<Transform>("Prefabs/blocks/foregrounds");
			foreach (var b in fgBlocks) {
				fgChain.AddNode(new MarkovChain<Transform>.Node(b));
			}

			fgChain.ConnectAllNoCycles();
			MarkovChain<Transform>.Node prevRoot = fgChain.Root;
			fgChain.Root = new MarkovChain<Transform>.Node(Resources.Load<Transform>("Prefabs/blocks/blockEmpty"));
			fgChain.Root.Neighbours.Add(prevRoot);
			fgChain.Current = fgChain.Root;

			bgChain.ConnectAll();

			GenerateBlock();
		}

		public void FixedUpdate() {
			if (Camera.main.transform.position.y + offsetY*1.25f> generatedCount * offsetY) {
				GenerateBlock();
			}
		}

	}
}
