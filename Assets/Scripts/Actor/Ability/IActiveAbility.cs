using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IActiveAbility : ISortable {

	void Update(float deltaTime);

	List<Transform> FindTarget();

	void Trigger(ActorInfo info, Utils.TriggerEffectDelegate triggerEffect);

	bool Interrupt();

	void Stop();

	void Resume();

	bool IsEnd();

	bool IsSuspend();

	bool IsAvailable();
	
	bool IsAotoTrigger();

	float GetMPCost();
	
	string UpdateAnimation();
}
