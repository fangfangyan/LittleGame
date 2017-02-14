using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroStateStroll : IHeroState {

	private readonly StatePatternHero hero;
	
	private HeroModel heroModel;

	private readonly IActorView actorView;
	
	private readonly IActorMovement movement;

	private LayerMask layerMask;
	
	public HeroStateStroll(StatePatternHero hero)
	{
		this.hero = hero;
		heroModel = hero.GetModel ();
		actorView = hero.GetActorView();
		movement = hero.GetMovement ();
		layerMask = Utils.GetPlayerFightMask ();
	}
	
	public void Update (float deltaTime)
	{
		UpdateMovement ();		
		UpdateView ();
		UpdateAbility (deltaTime);
		UpdateState ();
	}

	public void UpdateView()
	{
		actorView.ActorUpdate ();
	}

	public void UpdateAbility(float deltaTime)
	{
		if (null == heroModel.activeAbilities)
			return;
		foreach (IActiveAbility ability in heroModel.activeAbilities)
			ability.Update (deltaTime);
	}

	public void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Enemy"))
			ToNextState (EnumHeroState.Fight);

	}

//	public void OnCollisionEnter(Collision other)
//	{
//		if (other.gameObject.CompareTag ("Enemy"))
//			ToNextState (EnumHeroState.Fight);
//	}

	public void UpdateMovement()
	{
		movement.SetVelocity(heroModel.velocity);
		movement.SetDestination(heroModel.distination);
		movement.ActorUpdate ();
	}
	
	public void UpdateState()
	{
//		if(null != heroModel.skillAOEs && heroModel.skillAOEs.Count > 0)
//		{
//			ToNextState(EnumHeroState.Hurt);
//			return;
//		}
		if (FindEnimy ()) 
		{
			ToNextState (EnumHeroState.Fight);
			return;
		}

		if (null != heroModel.releaseAbilities) {
			ToNextState (EnumHeroState.Fight);
			return;
		}

	}

	private bool FindEnimy()
	{
		
		RaycastHit objhit;
		bool hasEnimy = Utils.SphereCast (hero.transform.localPosition, 
		                                  heroModel.DetectRange, 
		                                  hero.transform.forward, 
		                                  out objhit,
		                                  layerMask);
		return hasEnimy;
	}

	public void ToNextState(EnumHeroState state)
	{
		SetActive (false);
		hero.ToNextState (state);
	}
	
	public void SetTarget()
	{
		if (actorView != null)
			actorView.SetTarget (null);
	}
	
	public void SetActive(bool isActive)
	{
		if(isActive)
			SetTarget ();
	}
	
}
