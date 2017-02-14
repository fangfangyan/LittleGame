using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "AoeData", menuName = "AoeData", order = 111)]
public class AbilityAOEData : ScriptableObject {

	public string AOEName;

	public ActorBuffType Type;

	public float Power;

	public float Duration;

	public int TargetLimit;

	public float Speed;
}
