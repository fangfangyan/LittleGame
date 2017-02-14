using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroStateFight : IHeroState {

	public static readonly float LimitTimeForFightWithNoSkill = 10f;

	private readonly StatePatternHero hero;

	private readonly IActorView actorView;

	private HeroModel heroModel;

	private IActorMovement movement;

	private List<IActiveAbility> autoReleaseSkills;

//	private string animationName;

	private float timeToStroll;

	public HeroStateFight(StatePatternHero hero)
	{
		this.hero = hero;
		heroModel = hero.GetModel ();
		actorView = hero.GetActorView();
		movement = hero.GetMovement ();
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
		movement.SetVelocity(heroModel.velocity);
		movement.SetDestination(heroModel.distination);
		movement.ActorUpdate ();
	}
	
	public void UpdateState()
	{		
		if (UpdateBuffs ())
			return;
		
		if (heroModel.currentAbilities == null) 
		{
			if(timeToStroll < 0)
				timeToStroll = Time.time + LimitTimeForFightWithNoSkill;
			else if(Time.time >= timeToStroll)
				ToNextState(EnumHeroState.Stroll);
			return;
		}
		
		timeToStroll = -1;
	}

	public bool UpdateBuffs()
	{
		if (null == heroModel.skillAOEs || heroModel.skillAOEs.Count <= 0)
			return false;

		foreach (IAbilityAOE aoe in heroModel.skillAOEs) 
		{
			if (aoe.GetBuffType() == ActorBuffType.Blooding) 
			{
				if(heroModel.currentAbilities != null && heroModel.currentAbilities.Interrupt())
					heroModel.currentAbilities.Stop();

				hero.DecHp (aoe.GetPower ());

				if (heroModel.hp == 0)
					ToNextState (EnumHeroState.Dead);
				else
					ToNextState(EnumHeroState.Hurt);
				
				heroModel.skillAOEs.Remove (aoe);
				return true;
			}
		}			
		return false;
	}

	public void UpdateAbility(float deltaTime)
	{
		if (null == heroModel.activeAbilities)
			return;

		foreach (IActiveAbility ability in heroModel.activeAbilities)
			ability.Update (deltaTime);

		UpdatePlayerTrigger ();

		UpdateCurrentAbility ();

		AutoTriggerAbility ();
	}

	public void UpdatePlayerTrigger()
	{
		if (Time.time > heroModel.releaseTimeLimit) 
			heroModel.releaseAbilities = null;

		if (null == heroModel.releaseAbilities)
			return;

		if (heroModel.currentAbilities != null 
			&& heroModel.releaseAbilities.GetPower () <= heroModel.currentAbilities.GetPower ()) 
			return;

		if (heroModel.releaseAbilities.IsAvailable () && (heroModel.mp >= heroModel.releaseAbilities.GetPower ()))
			TriggerAbility (heroModel.releaseAbilities);

		heroModel.releaseAbilities = null;
	}

	public void UpdateCurrentAbility()
	{
		if (null == heroModel.currentAbilities)
			return;
			
		if (heroModel.currentAbilities.IsEnd ()) {
			heroModel.currentAbilities = null;
			return;
		}

		//TODO hero’s status, means normal or hurt or some other debuff that may be let it see diffrent
		//TODO may be use a callback to change hero’s animation
//		animationName = heroModel.currentAbilities.UpdateAnimation ();
	}

	public void AutoTriggerAbility()
	{
		if (null != heroModel.currentAbilities)
			return;

		if (null == autoReleaseSkills)
			autoReleaseSkills = new List<IActiveAbility> ();
		else
			autoReleaseSkills.Clear();

		float mp = heroModel.mp;
		foreach (IActiveAbility ability in heroModel.activeAbilities)
			if (ability.IsAotoTrigger () && ability.IsAvailable () && mp >= ability.GetMPCost ())
				autoReleaseSkills.Add (ability);
				
		if (autoReleaseSkills.Count <= 0) 
			return;

		autoReleaseSkills.Sort(Comparer<IActiveAbility>.GetInstance ());
		TriggerAbility (autoReleaseSkills[0]);
	}

	public void TriggerAbility(IActiveAbility ability)
	{
		if (heroModel.currentAbilities != null) 
		{
			heroModel.currentAbilities.Stop();
			heroModel.currentAbilities = null;
		}
		
		heroModel.currentAbilities = ability;
		
		ability.Trigger (heroModel.GetActorInfo (), TriggerEffect);
		
		SetTarget (ability.FindTarget());
	}

	public void GetHeroModel()
	{
		if (null == heroModel)
			heroModel = hero.GetModel ();
	}

	public void TriggerEffect(AbilityEffectData effectInfo)
	{
		if (effectInfo.EffectType == AbilityEffectData.Type.PlayActorAnimation)
			actorView.PlayAnimation (effectInfo.GoalName, effectInfo.GoalValue, false, null);
	}

	public void ToNextState(EnumHeroState state)
	{
		SetActive (false);
		hero.ToNextState (state);
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

//	public void OnCollisionEnter(Collision other)
//	{
////		if (other.gameObject.CompareTag ("Enemy"))
////			ToNextState (EnumHeroState.Fight);
//	}

	public void SetActive(bool isActive)
	{

	}

}
