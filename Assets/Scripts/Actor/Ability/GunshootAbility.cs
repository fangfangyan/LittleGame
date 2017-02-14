using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunshootAbility : IActiveAbility 
{
	private readonly AbilityData skillModel;

	private readonly Transform actorTransform;

	private readonly Transform bulletTransform;

	private readonly LayerMask layerMask;

	private List<IAbilityEffect> effects;

	private List<Transform> targets;

	private EAbilityState currentState;

	private float nextStateTime;

	private float currentStateTime;

	private List<IAbilityEffect> beforeAttackedEffect;

	private List<IAbilityEffect> attackingEffect;

	private List<IAbilityEffect> afterAttackedEffect;

	private List<IAbilityEffect> coolDownEffect;

	private EAbilityState suspendState;

	private ActorInfo actorInfo;

	private Utils.TriggerEffectDelegate triggerEffect;

	public GunshootAbility(AbilityData skillModel, Transform actorTransform)
	{
		this.skillModel = skillModel;

		this.actorTransform = actorTransform;

		bulletTransform = Utils.GetChildByName (actorTransform, "bullet");

		layerMask = Utils.GetMaskByTypes (skillModel.TargetTypes);

		effects = new List<IAbilityEffect> ();

		beforeAttackedEffect = new List<IAbilityEffect> ();
		
		attackingEffect = new List<IAbilityEffect> ();
		
		afterAttackedEffect = new List<IAbilityEffect> ();

		coolDownEffect = new List<IAbilityEffect> ();
	}

	public void AddEffect(IAbilityEffect effect)
	{
		effects.Add (effect);
	}

	public void Update(float deltaTime)
	{
		currentStateTime += deltaTime;
		switch (currentState) 
		{
		case EAbilityState.Standby:
			break;
		case EAbilityState.BeforeAttack:
			UpdateEffect(beforeAttackedEffect);
			if(currentStateTime >= nextStateTime)
				GotoNextState(EAbilityState.Attacking);
			break;
		case EAbilityState.Attacking:
			UpdateEffect(attackingEffect);
			if(currentStateTime >= nextStateTime)
				GotoNextState(EAbilityState.AfterAttack);
			break;
		case EAbilityState.AfterAttack:
			UpdateEffect(afterAttackedEffect);
			if(currentStateTime >= nextStateTime)
				Stop ();
			break;
		case EAbilityState.Suspend:
			break;
		case EAbilityState.CoolDown:
			UpdateEffect(coolDownEffect);
			if(currentStateTime >= nextStateTime)
				GotoNextState(EAbilityState.Standby);
			break;
		}
	}

	public void UpdateEffect(List<IAbilityEffect> stateEffects)
	{
		int count = stateEffects.Count;
		for(int i = 0; i < count; i++)
		{
			IAbilityEffect effect = stateEffects[i];
			if(effect.GetTrigerTime() <= currentStateTime)
			{
				effect.Trigger(actorInfo, bulletTransform, triggerEffect);
				stateEffects.Remove(effect);
				count--;
				i--;
			}
		}
	}

	public void GotoNextState(EAbilityState nextState)
	{
		currentState = nextState;
		currentStateTime = 0;

		switch (nextState) 
		{
		case EAbilityState.BeforeAttack:
			nextStateTime = skillModel.StateTimes[(int)nextState];
			SetEffect(EAbilityState.BeforeAttack, beforeAttackedEffect);
			break;
		case EAbilityState.Attacking:
			nextStateTime = skillModel.StateTimes[(int)nextState];
			SetEffect(EAbilityState.Attacking, attackingEffect);
			break;
		case EAbilityState.AfterAttack:
			nextStateTime = skillModel.StateTimes[(int)nextState];
			SetEffect(EAbilityState.AfterAttack, afterAttackedEffect);
			break;
		case EAbilityState.CoolDown:
			nextStateTime = skillModel.StateTimes[(int)nextState];
			SetEffect(EAbilityState.CoolDown, coolDownEffect);
			break;
		}
	}

	public List<Transform> FindTarget()
	{
		if (null == targets)
			targets = new List<Transform> ();
		else
			targets.Clear ();

		RaycastHit objhit;
		if( Utils.SphereCast (actorTransform.localPosition, 
		                      skillModel.AttackRange, 
		                      actorTransform.forward, 
		                      out objhit,
		                      layerMask))
		{
			targets.Add (objhit.collider.gameObject.transform);
		}

		return targets;
	}
	
	public void Trigger(ActorInfo actorInfo, Utils.TriggerEffectDelegate triggerEffect)
	{
		this.actorInfo = actorInfo;
		this.triggerEffect = triggerEffect;
		GotoNextState (EAbilityState.BeforeAttack);
	}

	public void SetEffect(EAbilityState state, List<IAbilityEffect> stateEffects){
		foreach (IAbilityEffect effect in effects)
			if (effect.GetState().Equals (state))
				stateEffects.Add (effect);
	}

	public bool Interrupt()
	{
		if (!skillModel.CanInterrupted)
			return false;
		suspendState = currentState;
		currentState = EAbilityState.Suspend;
		return true;
	}
	
	public void Stop()
	{
		GotoNextState (EAbilityState.CoolDown);
		beforeAttackedEffect.Clear ();
		attackingEffect.Clear ();
		afterAttackedEffect.Clear ();
	}
	
	public void Resume()
	{
		currentState = suspendState;
	}
	
	public bool IsEnd()
	{
		if (currentState.Equals (EAbilityState.BeforeAttack) 
			|| currentState.Equals (EAbilityState.Attacking) 
			|| currentState.Equals (EAbilityState.AfterAttack))
			return false;
		return true;
	}
	
	public bool IsSuspend()
	{
		return currentState.Equals (EAbilityState.Suspend);
	}
	
	public bool IsAvailable()
	{
		if (currentState != EAbilityState.Standby)
			return false;
		List<Transform> targets = FindTarget ();
		if (null == targets || targets.Count <= 0)
			return false;
		return true;
	}
	
	public bool IsAotoTrigger()
	{
		return skillModel.CanAutoTrigger;
	}
	
	public float GetMPCost()
	{
		return skillModel.MpConsume;
	}
	
	public string UpdateAnimation()
	{
		return null;
	}

	public int GetPower()
	{
		return skillModel.StatePowers[(int)currentState];
	}
	
}
