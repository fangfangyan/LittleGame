using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierModel {

	public readonly ActorData soldierData;
	
	public readonly string actorId;
	
	public HeroCamp camp;
	
	public ECamp campType;
	
	public float DetectRange;
	
	public float hp;
	
	public float mp;
	
	public Vector3 velocity;
	
	public Vector3 distination;
	
	public List<IActiveAbility> activeAbilities;
	
	public IActiveAbility currentAbilities;

	public List<IAbilityAOE> skillAOEs;
	
	public List<IAbilityAOE> skillBuffs;
	
	public SoldierModel(ActorData soldierData)
	{
		this.soldierData = soldierData;
		this.actorId = soldierData.ActorId;
		DetectRange = soldierData.DetectRange;
		hp = soldierData.MaxHp;
		mp = soldierData.MaxMp;
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
