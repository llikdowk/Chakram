using System;
using System.Collections.Generic;

public enum Tag {
	Untagged, Respawn, Finish, EditorOnly, MainCamera, Player, GameController, // default
	// custom
}

class TagManager {

	private static TagManager instance;
	private readonly Dictionary<Tag, string> map;

	private TagManager() {
		map = new Dictionary<Tag, string>();
		foreach (Tag item in Enum.GetValues(typeof(Tag))) {
			AddTag(item, item.ToString());
		}
	}

	private void AddTag(Tag tag, string name) {
		map.Add(tag, name);
	}

	public static string Get(Tag tag) {
		if (instance == null) instance = new TagManager();
		return instance.map[tag];
	}
}
