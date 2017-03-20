using CustomDebug;
using UnityEngine;

namespace Game.Utils {
	public class BlockStartUtils : MonoBehaviour {
		private GameObject securityZone;

		public void Awake() {
			foreach (var child in gameObject.GetComponentsInChildren<Transform>()) {
				if (child.name == "securityZone") {
					securityZone = child.gameObject;
				}
			}
			if (!securityZone) {
				DebugMsg.ChildObjectNotFound(Debug.LogError, "securityZone");
			}
		}

		public void DisableSecurityZone() {
			securityZone.SetActive(false);
		}
	}
}
