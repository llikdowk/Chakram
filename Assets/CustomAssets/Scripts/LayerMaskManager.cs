using System;
using System.Collections.Generic;
using UnityEngine;

public enum Layer {
	Default, TransparentFX, IgnoreRaycast, Water, UI, // default
	Player, Line // custom
}

public class LayerMaskManager {
	private static LayerMaskManager instance;
	private readonly Dictionary<Layer, int> map;

	private LayerMaskManager() {
		map = new Dictionary<Layer, int>();
		foreach (Layer item in Enum.GetValues(typeof(Layer))) {
			AddLayer(item);
		}
	}

	private void AddLayer(Layer layer) {
		string name = layer.ToString();
		if (layer == Layer.IgnoreRaycast) {
			name = "Ignore Raycast";
		}
		map.Add(layer, LayerMask.NameToLayer(name));
	}

	public static int Get(Layer layer) {
		if (instance == null) instance = new LayerMaskManager();
		return instance.map[layer];
	}

}
