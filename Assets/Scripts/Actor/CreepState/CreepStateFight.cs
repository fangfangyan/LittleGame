using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepStateFight : ICreepState {

	public static readonly float LimitTimeForFightWithNoSkill = 10f;
	
	private readonly StatePatternCreep creep;
	
	private CreepModel creepModel;
	
	private readonly IActorView actorView;
	
	private readonly IActorMovement movement;
	
	private List<IActiveAbility> autoReleaseSkills;
	
//	private string animationName;
	
	private float timeToStroll;

	private Transform targetTransform;
	
	public CreepStateFight(StatePatternCreep creep)
	{
		this.creep = creep;
		creepModel = creep.GetModel ();
		actorView = creep.GetActorView();
		movement = creep.GetMovement ();
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
//			actorView.PlayAnimation();
//			animationName = null;
//		}
		actorView.ActorUpdate ();
	}
	
	public void UpdateMovement()
	{
		if (null != targetTransform && targetTransform.gameObject.activeSelf) 
		{
			Vector3 distance = creep.transform.localPosition - targetTransform.localPosition;

			if (distance.magnitude < creepModel.FightRange)
				movement.SetDestination(creep.transform.localPosition);
			else
				movement.SetDestination(targetTransform.localPosition);
		}
		movement.ActorUpdate ();
	}
	
	public void UpdateState()
	{
		if (UpdateBuffs ())
			return;
		
		if (creepModel.currentAbilities == null) 
		{
			if(timeToStroll < 0)
				timeToStroll = Time.time + LimitTimeForFightWithNoSkill;
			else if(Time.time >= timeToStroll)
				ToNextState(EnumCreepState.Stroll);
			return;
		}
		
		timeToStroll = -1;
	}

	public bool UpdateBuffs()
	{
		if (null == creepModel.skillAOEs || creepModel.skillAOEs.Count <= 0)
			return false;

		foreach (IAbilityAOE aoe in creepModel.skillAOEs) 
		{
			if (aoe.GetBuffType() == ActorBuffType.Blooding) 
			{
				if(creepModel.currentAbilities != null && creepModel.currentAbilities.Interrupt())
					creepModel.currentAbilities.Stop();

				creep.DecHp (aoe.GetPower ());

				if (creepModel.hp == 0)
					ToNextState (EnumCreepState.Dead);
				else
					ToNextState(EnumCreepState.Hurt);

				creepModel.skillAOEs.Remove (aoe);
				return true;
			}
		}			
		return false;
	}

	public void UpdateAbility(float deltaTime)
	{
		if (null == creepModel.activeAbilities)
			return;
		
		foreach (IActiveAbility ability in creepModel.activeAbilities)
			ability.Update (deltaTime);
		
		UpdateCurrentAbility ();
		
		AutoTriggerAbility ();
	}
	
	public void UpdateCurrentAbility()
	{
		if (null == creepModel.currentAbilities)
			return;
		
		if (creepModel.currentAbilities.IsEnd ()) {
			creepModel.currentAbilities = null;
			return;
		}
		
		//TODO hero’s status, means normal or hurt or some other debuff that may be let it see diffrent
		//TODO may be use a callback to change hero’s animation
//		animationName = creepModel.currentAbilities.UpdateAnimation ();
	}
	
	public void AutoTriggerAbility()
	{
		if (null != creepModel.currentAbilities)
			return;
		
		if (null == autoReleaseSkills)
			autoReleaseSkills = new List<IActiveAbility> ();
		else
			autoReleaseSkills.Clear();
		
		float mp = creepModel.mp;
		foreach (IActiveAbility ability in creepModel.activeAbilities)
			if (ability.IsAotoTrigger () && ability.IsAvailable () && mp >= ability.GetMPCost ())
				autoReleaseSkills.Add (ability);
		
		if (autoReleaseSkills.Count <= 0) 
			return;
		
		autoReleaseSkills.Sort(Comparer<IActiveAbility>.GetInstance ());
		TriggerAbility (autoReleaseSkills[0]);
	}
	
	public void TriggerAbility(IActiveAbility ability)
	{
		if (creepModel.currentAbilities != null) 
		{
			creepModel.currentAbilities.Stop();
			creepModel.currentAbilities = null;
		}
		
		creepModel.currentAbilities = ability;
		
		ability.Trigger (creepModel.GetActorInfo (), TriggerEffect);
		
		SetTarget (ability.FindTarget());
	}
	
	public void GetCreepModel()
	{
		if (null == creepModel)
			creepModel = creep.GetModel ();
	}
	
	public void TriggerEffect(AbilityEffectData effectInfo)
	{
		if (effectInfo.EffectType == AbilityEffectData.Type.PlayActorAnimation)
			actorView.PlayAnimation (effectInfo.GoalName, effectInfo.GoalValue, false, null);
	}
	
	public void ToNextState(EnumCreepState state)
	{
		SetActive (false);
		creep.ToNextState (state);
	}
	
	public void SetTarget(List<Transform> target)
	{
		targetTransform = target [0];
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
		
	}

}
