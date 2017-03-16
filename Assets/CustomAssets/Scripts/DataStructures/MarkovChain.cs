

using UnityEngine;

namespace Assets.CustomAssets.Scripts.DataStructures {
	public class MarkovChain<T> where T : Object {
		public class Node {
			public T Value;
			public C5.ArrayList<Node> Neighbours = new C5.ArrayList<Node>();

			public Node(T value) {
				this.Value = value;
			}

		}

		private readonly C5.C5Random randomizer = new C5.C5Random();
		private readonly C5.ArrayList<Node> nodes = new C5.ArrayList<Node>();
		public Node Current;
		public Node Root; 

		public void AddNode(Node n) {
			if (Current == null) {
				Current = n;
				Root = n;
			}
			nodes.Add(n);
		}

		/// <summary>
		/// Multidirectional link
		/// </summary>
		/// <param name="n1"></param>
		/// <param name="n2"></param>
		public void AddEdge(Node n1, Node n2) {
			AddArrow(n1, n2);
			AddArrow(n2, n1);
		}

		/// <summary>
		/// Unidirectional link
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public void AddArrow(Node from, Node to) {
			from.Neighbours.Add(to);
		}

		public void AddCycle(Node n) {
			AddArrow(n, n);
		}

		public T Next() {
			Node n = Current;
			Current = Current.Neighbours[randomizer.Next(0, Current.Neighbours.Count)];;
			return Object.Instantiate(n.Value);
		}

		public void ConnectAllNoCycles() {
			foreach (var n in nodes) {
				foreach (var m in nodes) {
					if (n != m) {
						AddArrow(n, m);
					}
				}
			}
		}

		public void ConnectAll() {
			foreach (var n in nodes) {
				foreach (var m in nodes) {
					AddArrow(n, m);
				}
			}
		}
	}
}
