using UnityEngine;
using System.Collections;

public class AbilityEffectTriggerAOE : IAbilityEffect {
	
	private readonly AbilityEffectData info;

	public AbilityEffectTriggerAOE(AbilityEffectData info)
	{
		this.info = info;
	}

	public EAbilityState GetState()
	{
		return info.AbilityState;
	}
	
	public float GetTrigerTime()
	{
		return info.TriggeredTime;
	}

	public void Trigger(ActorInfo actorInfo, Transform transform, Utils.TriggerEffectDelegate triggerEffect)
	{
		ActorManager.GetInstance ().CreateAOE (info.GoalName, actorInfo, transform);
	}
	
	public AbilityEffectData GetEffectInfo()
	{
		return info;
	}
		
}
