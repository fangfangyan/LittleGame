using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "AbilityData", menuName = "AbilityData", order = 111)]
public class AbilityData : ScriptableObject {

	public enum Type{Gunshoot, Melee}

	public Type SkillType;

	public string SkillId;

	public string SkillName;

	public float MpConsume;

	public float AttackRange;

	public List<ECamp> TargetTypes;

	public float[] StateTimes;

	public int[] StatePowers;

	public bool CanInterrupted;

	public bool CanAutoTrigger;

	public List<AbilityEffectData> effectInfos;

}
