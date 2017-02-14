using UnityEngine;
using System.Collections;

public interface IHeroControl {

	///
	///Pass the velocity to the IHeroControl
	void SetVelocity(Vector3 velocity);

	void SetDestination(Vector3 destination);

	void TriggerAbility(IActiveAbility skill);

	IActiveAbility[] GetAvailableSkill();
	
}
