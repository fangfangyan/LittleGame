using UnityEngine;
using System.Collections;

public interface IAbilityEffect {

	EAbilityState GetState();

	float GetTrigerTime();

	void Trigger(ActorInfo info, Transform transform, Utils.TriggerEffectDelegate triggerEffect);

	AbilityEffectData GetEffectInfo();
}
