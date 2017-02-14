using UnityEngine;
using System.Collections;

public interface IAbilityAOE {

	void SetAOEInfo(AbilityAOEData data);

	void SetActorInfo(ActorInfo actorInfo);

	void OnActorEnter(GameObject actor);

	ActorBuffType GetBuffType ();

	float GetPower ();

	Transform GetTransform();
}
