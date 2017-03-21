using UnityEngine;
using UnityEngine.UI;

class Loader : MonoBehaviour {
	private bool isLoaded;

	public void Awake() {
		GetComponent<Image>().enabled = true;
	}

	public void Start() {
	}

	public void Update() {
		if (isLoaded) {
		GetComponent<Image>().enabled = false;
			Object.Destroy(this);
		}
		isLoaded = true;
	}
}
