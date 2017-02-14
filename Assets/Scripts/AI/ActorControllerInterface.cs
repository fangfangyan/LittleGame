using UnityEngine;
using System.Collections;

public interface ActorControllerInterface {

	void SetPosition(Vector3 position);

	//set actor’s movement follow the previous
	void FollowTheActor(Actor previous);

	//set actor’s movement auto navigation
	void SetAutoNav();

	//set actor’s velocity
	void SetVelocity(Vector3 velocity);

	void LaunchSkill(string skillId);

	void AddDamage(int damage);

	void SetHP(int hp);

	void SetActorHurt(); 

	void SetActorDie();
}