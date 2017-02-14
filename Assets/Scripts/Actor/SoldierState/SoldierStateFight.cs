using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierStateFight : ISoldierState {

	public static readonly float LimitTimeForFightWithNoSkill = 10f;

	private readonly StatePatternSoldier soldier;
	
	private SoldierModel soldierModel;
	
	private IActorView actorView;
	
	private IActorMovement movement;

	private List<IActiveAbility> autoReleaseSkills;
	
//	private string animationName;
	
	private float timeToStroll;

	public SoldierStateFight(StatePatternSoldier soldier)
	{
		this.soldier = soldier;
		soldierModel = soldier.GetModel ();
		actorView = soldier.GetActorView();
		movement = soldier.GetMovement ();
	}

	public void Update (float deltaTime)
	{
		UpdateAbility (deltaTime);
		UpdateMovement ();
		UpdateView ();
		UpdateState ();
	}
	
	public void UpdateView()
	{
//		if (null != animationName) 
//		{
//			actorView.PlayAnimation(animationName);
//			animationName = null;
//		}
		actorView.ActorUpdate ();
	}
	
	public void UpdateMovement()
	{
		movement.ActorUpdate ();
	}
	
	public void UpdateState()
	{
		if (UpdateBuffs ())
			return;
		
		if (soldierModel.currentAbilities == null) 
		{
			if(timeToStroll < 0)
				timeToStroll = Time.time + LimitTimeForFightWithNoSkill;
			else if(Time.time >= timeToStroll)
				ToNextState(EnumSoldierState.Stroll);
			return;
		}
		
		timeToStroll = -1;
	}

	public bool UpdateBuffs()
	{
		if (null == soldierModel.skillAOEs || soldierModel.skillAOEs.Count <= 0)
			return false;

		foreach (IAbilityAOE aoe in soldierModel.skillAOEs) 
		{
			if (aoe.GetBuffType() == ActorBuffType.Blooding) 
			{
				if(soldierModel.currentAbilities != null && soldierModel.currentAbilities.Interrupt())
					soldierModel.currentAbilities.Stop();

				soldier.DecHp (aoe.GetPower ());

				if (soldierModel.hp == 0)
					ToNextState (EnumSoldierState.Dead);
				else
					ToNextState(EnumSoldierState.Hurt);

				soldierModel.skillAOEs.Remove (aoe);
				return true;
			}
		}			
		return false;
	}

	public void UpdateAbility(float deltaTime)
	{
		if (null == soldierModel.activeAbilities)
			return;
		
		foreach (IActiveAbility ability in soldierModel.activeAbilities)
			ability.Update (deltaTime);
		
		UpdateCurrentAbility ();
		
		AutoTriggerAbility ();
	}
	
	public void UpdateCurrentAbility()
	{
		if (null == soldierModel.currentAbilities)
			return;
		
		if (soldierModel.currentAbilities.IsEnd ()) {
			soldierModel.currentAbilities = null;
			return;
		}
		
		//TODO hero’s status, means normal or hurt or some other debuff that may be let it see diffrent
		//TODO may be use a callback to change hero’s animation
//		animationName = soldierModel.currentAbilities.UpdateAnimation ();
	}
	
	public void AutoTriggerAbility()
	{
		if (null != soldierModel.currentAbilities)
			return;
		
		if (null == autoReleaseSkills)
			autoReleaseSkills = new List<IActiveAbility> ();
		else
			autoReleaseSkills.Clear();
		
		float mp = soldierModel.mp;
		foreach (IActiveAbility ability in soldierModel.activeAbilities)
			if (ability.IsAotoTrigger () && ability.IsAvailable () && mp >= ability.GetMPCost ())
				autoReleaseSkills.Add (ability);
		
		if (autoReleaseSkills.Count <= 0) 
			return;
		
		autoReleaseSkills.Sort(Comparer<IActiveAbility>.GetInstance ());
		TriggerAbility (autoReleaseSkills[0]);
	}
	
	public void TriggerAbility(IActiveAbility ability)
	{
		if (soldierModel.currentAbilities != null) 
		{
			soldierModel.currentAbilities.Stop();
			soldierModel.currentAbilities = null;
		}
		
		soldierModel.currentAbilities = ability;
		
		ability.Trigger (soldierModel.GetActorInfo (), TriggerEffect);
		
		SetTarget (ability.FindTarget());
	}
	
	public void GetSoldierModel()
	{
		if (null == soldierModel)
			soldierModel = soldier.GetModel ();
	}

	public void TriggerEffect(AbilityEffectData effectInfo)
	{
		if (effectInfo.EffectType == AbilityEffectData.Type.PlayActorAnimation)
			actorView.PlayAnimation (effectInfo.GoalName, effectInfo.GoalValue, false, null);
	}

	public void ToNextState(EnumSoldierState state)
	{
		SetActive (false);
		soldier.ToNextState (state);
	}
	
	public void SetTarget(List<Transform> target)
	{
		if (actorView != null)
			actorView.SetTarget (target[0]);
	}
	
	public void OnTriggerEnter (Collider other)
	{
		//		if (other.gameObject.CompareTag ("Enemy"))
		//			ToNextState (EnumHeroState.Fight);
		//		
	}
	
	public void SetActive(bool isActive)
	{
		if(true)
			movement = soldier.GetMovement ();
	}

}
