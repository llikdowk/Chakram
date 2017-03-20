
using Assets.CustomAssets.Scripts.DataStructures;
using Game.Utils;
using UnityEngine;

namespace Game.Background {
	public class BackgroundGenerator : MonoBehaviour {
		private static readonly C5.CircularQueue<Transform> instancedBlocks = new C5.CircularQueue<Transform>(4);
		private static int generatedCount = 0;
		private const float offsetY = 19.2f;
		private static MarkovChain<Transform> fgChain;
		private static MarkovChain<Transform> bgChain;
		private Transform blockStart;

		private void GenerateBlock() {
			Transform block = new GameObject("^block" + generatedCount).transform;
			Transform bg = bgChain.Next();
			bg.parent = block;
			bg.localPosition = Vector3.zero;
			Transform fg;
			fg = fgChain.Next();
			fg.parent = block;
			fg.localPosition = Vector3.zero;

			block.transform.parent = transform;
			block.transform.position = Vector3.up * generatedCount * offsetY;
			BackgroundColorManager.Register(block.gameObject);
			++generatedCount;
			instancedBlocks.Enqueue(block);
			if (instancedBlocks.Count == 4) {
				Transform old = instancedBlocks.Dequeue();
				BackgroundColorManager.Unregister(old.gameObject);
				Object.Destroy(old.gameObject);
			}
		}

		public static void Reset() {
			fgChain.Current = fgChain.Root;
			generatedCount = 0;
		}

		public static void ExitSafeZone() {
			foreach (var block in instancedBlocks) {
				BlockStartUtils startUtils = block.GetComponentInChildren<BlockStartUtils>();
				if (startUtils) {
					startUtils.DisableSecurityZone();
				}
			}
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

			var emptyBlock = Resources.Load<Transform>("Prefabs/blocks/blockEmpty");
			MarkovChain<Transform>.Node prevRoot = fgChain.Root;
			prevRoot.Neighbours.Add(prevRoot);
			fgChain.Root = new MarkovChain<Transform>.Node(emptyBlock);
			fgChain.Root.Neighbours = prevRoot.Neighbours;
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
