using System;
using System.Collections.Generic;
using UnityEngine;

public enum Layer {
	Default, TransparentFX, IgnoreRaycast, Water, UI, // default
	Player // custom
}

public class LayerMaskManager {
	private static LayerMaskManager _instance;
	private readonly Dictionary<Layer, int> _map;

	private LayerMaskManager() {
		_map = new Dictionary<Layer, int>();
		foreach (Layer item in Enum.GetValues(typeof(Layer))) {
			AddLayer(item);
		}
	}

	private void AddLayer(Layer layer) {
		string name = layer.ToString();
		if (layer == Layer.IgnoreRaycast) {
			name = "Ignore Raycast";
		}
		_map.Add(layer, LayerMask.NameToLayer(name));
	}

	public static int Get(Layer layer) {
		if (_instance == null) _instance = new LayerMaskManager();
		return _instance._map[layer];
	}

}
