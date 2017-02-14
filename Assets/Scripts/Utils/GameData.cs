using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "GameData", menuName = "GameData", order = 111)]
public class GameData : ScriptableObject {
	
	public Vector3 heroLocation;

	public List<Vector3> creepLocations;

	public List<Vector3> soldierLocations;

}
