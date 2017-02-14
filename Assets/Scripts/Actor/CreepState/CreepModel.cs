using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepModel {

	public readonly ActorData creepData;
	
	public readonly string actorId;
	
	public ECamp campType;
	
	public float DetectRange;

	public float FightRange;
	
	public float hp;
	
	public float mp;
	
	public Vector3 velocity;
	
	public Vector3 distination;
	
	public List<IActiveAbility> activeAbilities;
	
	public IActiveAbility currentAbilities;
	
	public List<IAbilityAOE> skillAOEs;
	
	public List<IAbilityAOE> skillBuffs;
	
	public CreepModel(ActorData creepData)
	{
		this.creepData = creepData;
		this.actorId = creepData.ActorId;
		DetectRange = creepData.DetectRange;
		FightRange = creepData.FightRange;
		hp = creepData.MaxHp;
		mp = creepData.MaxMp;
		campType = creepData.camp;
		activeAbilities = new List<IActiveAbility> ();
		skillAOEs = new List<IAbilityAOE> ();
		skillBuffs = new List<IAbilityAOE> ();
	}
	
	public void AddAbility(IActiveAbility ability)
	{
		activeAbilities.Add (ability);
	}

	public void AddAbilityAOE(IAbilityAOE aoe)
	{
		if (skillAOEs.Contains (aoe))
			return;
		skillAOEs.Add (aoe);
	}
	
	public ActorInfo GetActorInfo()
	{
		return new ActorInfo (this);
	}
}
