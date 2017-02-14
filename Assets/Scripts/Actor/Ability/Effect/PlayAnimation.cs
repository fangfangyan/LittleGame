using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : IAbilityEffect {

	private readonly AbilityEffectData info;

	public PlayAnimation(AbilityEffectData info)
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
		triggerEffect.Invoke (info);
	}

	public AbilityEffectData GetEffectInfo()
	{
		return info;
	}

}
