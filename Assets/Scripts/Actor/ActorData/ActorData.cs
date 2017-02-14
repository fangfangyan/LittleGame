using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "ActorData", menuName = "ActorData", order = 111)]
public class ActorData : ScriptableObject {

	public string ActorId;

	public ECamp camp;

	public float DetectRange;

	public float FightRange;
	
	public float MaxHp;
	
	public float MaxMp;
	
	public List<AbilityData> Abilities;

}
