using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "GameInfo", menuName = "GameInfo", order = 111)]
public class GameInfo : ScriptableObject {

	public Vector3 heroLocation;

	public List<Vector3> creepLocations;

	public List<Vector3> soldierLocations;

}

