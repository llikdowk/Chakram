using System;
using System.Collections.Generic;

public enum Tag {
	Untagged, Respawn, Finish, EditorOnly, MainCamera, Player, GameController, // default
	// custom
}

class TagManager {

	private static TagManager _instance;
	private readonly Dictionary<Tag, string> _map;

	private TagManager() {
		_map = new Dictionary<Tag, string>();
		foreach (Tag item in Enum.GetValues(typeof(Tag))) {
			AddTag(item, item.ToString());
		}
	}

	private void AddTag(Tag tag, string name) {
		_map.Add(tag, name);
	}

	public static string Get(Tag tag) {
		if (_instance == null) _instance = new TagManager();
		return _instance._map[tag];
	}
}
