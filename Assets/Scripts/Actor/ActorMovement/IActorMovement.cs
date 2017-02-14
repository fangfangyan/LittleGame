using UnityEngine;
using System.Collections;

public interface IActorMovement {
	
	void SetVelocity(Vector3 velocity);

	Vector3 GetVelocity();
	
	void SetDestination(Vector3 destination);

	void ActorUpdate();

}
