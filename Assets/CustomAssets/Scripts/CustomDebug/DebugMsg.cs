
namespace CustomDebug {
	public static class DebugMsg {
		public delegate void DebugLog(string msg);

		private const string sep = " <color=grey>##</color> ";

		public static void ComponentNotFound(DebugLog log, System.Type component, string extraInfo = "") {
			log("Component <b>" + component + "</b> not found" + sep + extraInfo);
		}

		public static void GameObjectNotFound(DebugLog log, string gameObjectName, string extraInfo = "") {
			log("GameObject with name <b>" + gameObjectName + "</b> not found" + sep + extraInfo);
		}

		public static void NoExistantInteraction(DebugLog log) {
			log("Trying to remove a non-existant interaction");
		}

		public static void SingletonNotCreated(DebugLog log) {
			log("Singleton has not been created!");
		}

		public static void ChildObjectNotFound(DebugLog log, string childObjectName) {
			log("Child object <b>" + childObjectName + "</b> not found in current gameObject");
		}

		public static void ResourceNotFound(DebugLog log, string resourcePath) {
			log("Resource <b>" + resourcePath + "</b> not found");
		}
	}
}
