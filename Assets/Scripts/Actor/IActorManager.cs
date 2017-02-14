using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IActorManager : INode<IActorManager>{

	/// <summary>
	/// Gets the actor identifier.
	/// </summary>
	/// <returns>The actor identifier.</returns>
	string GetActorId();

	/// <summary>
	/// Gets the type of the camp.
	/// </summary>
	/// <returns>The camp type.</returns>
	ECamp GetCampType();

	void SetCamp(HeroCamp camp);

	/// <summary>
	/// Gets the camp.
	/// </summary>
	/// <returns>The camp.</returns>
	HeroCamp GetCamp();

	/// <summary>
	/// Beens the attacted.
	/// </summary>
	/// <returns>The attact result.</returns>
	/// <param name="attacker">Attacker.</param>
	/// <param name="skicssssll">Skill.</param>
	EAttackResult BeenAttacted(IAbilityAOE buff);

	/// <summary>
	/// Adds the hp.
	/// </summary>
	/// <param name="hpIncrement">Hp increment.</param>
	void AddHp(float hpIncrement);

	/// <summary>
	/// Decs the hp.
	/// </summary>
	/// <param name="hpDecrement">Hp decrement.</param>
	void DecHp(float hpDecrement);

	/// <summary>
	/// Adds the mp.
	/// </summary>
	/// <param name="mpIncrement">Mp increment.</param>
	void AddMp(float mpIncrement);

	/// <summary>
	/// Decs the mp.
	/// </summary>
	/// <param name="mpDecrement">Mp decrement.</param>
	void DecMp(float mpDecrement);

//	/// <summary>
//	/// Recruits the soldier.
//	/// </summary>
//	/// <returns><c>true</c>, If the recruitment succeeded, <c>false</c> otherwise.</returns>
//	/// <param name="soldier">Soldier.</param>
//	bool RecruitSoldier(IActorManager soldier);

	/// <summary>
	/// Beens the recruited.
	/// </summary>
	/// <returns><c>true</c>, If the recruitment succeeded, <c>false</c> otherwise.</returns>
	/// <param name="camp">Camp.</param>
	bool BeenRecruited(HeroCamp camp);

	/// <summary>
	/// Died this instance.
	/// </summary>
	void Died();

	GameObject GetGameObject();

	IActorMovement GetMovement();

}