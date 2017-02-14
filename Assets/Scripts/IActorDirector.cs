using UnityEngine;
using System.Collections;

public interface IActorDirector {

	/// <summary>
	/// Attacks the actor.
	/// </summary>
	/// <param name="skill">Skill.</param>
	/// <param name="actor">Actor.</param>
	void AttackActor(IAbilityAOE buff, IActorManager actor);

	/// <summary>
	/// Attacks the actor.
	/// </summary>
	/// <param name="hp">Hp.</param>
	/// <param name="actor">Actor.</param>
	void AttackActor(float damage, IActorManager actor);

	/// <summary>
	/// Recruits the soldier.
	/// </summary>
	/// <returns><c>true</c>, if soldier was recruited, <c>false</c> otherwise.</returns>
	/// <param name="camp">Camp.</param>
	/// <param name="soldier">Soldier.</param>
	void RecruitSoldier(HeroCamp camp, IActorManager soldier);

	/// <summary>
	/// Adds the hp.
	/// </summary>
	/// <param name="hpIncrement">Hp increment.</param>
	void AddActorHp(float hpIncrement, IActorManager actor);
	
	/// <summary>
	/// Decs the hp.
	/// </summary>
	/// <param name="hpDecrement">Hp decrement.</param>
	void DecActorHp(float hpDecrement, IActorManager actor);
	
	/// <summary>
	/// Adds the mp.
	/// </summary>
	/// <param name="mpIncrement">Mp increment.</param>
	void AddActorMp(float mpIncrement, IActorManager actor);
	
	/// <summary>
	/// Decs the mp.
	/// </summary>
	/// <param name="mpDecrement">Mp decrement.</param>
	void DecActorMp(float mpDecrement, IActorManager actor);
	
	/// <summary>
	/// Died this instance.
	/// </summary>
	void ActorDied(IActorManager actor);

}
