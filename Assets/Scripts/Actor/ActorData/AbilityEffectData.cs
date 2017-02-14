using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(fileName = "EffectData", menuName = "EffectData", order = 111)]
public class AbilityEffectData : ScriptableObject{

	public enum Type{PlayActorAnimation, PlayEffectAnimation, PlaySound, TriggerSkill, TriggerAOE}

	public Type EffectType;

	public EAbilityState AbilityState;

	public float TriggeredTime;

	public string GoalName;

	public float GoalValue;

}
