using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class InsResources{
	
	private static Dictionary<string, GameObject> prefabMap;

	private static GameObject getPrefabByName(string prefabName)
	{
		if (null == prefabMap)
			prefabMap = new Dictionary<string, GameObject> ();

		GameObject prefab;

		if (prefabMap.TryGetValue (prefabName, out prefab))
			return prefab;

		prefab = Resources.Load(prefabName) as GameObject;
		prefabMap.Add (prefabName, prefab);

		return prefab;
	}

	public static GameObject InsGameObject(string prefabName){
		GameObject prefab = getPrefabByName (prefabName);
		return MonoBehaviour.Instantiate (prefab); 
	}
}
