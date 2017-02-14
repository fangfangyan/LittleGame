using UnityEngine;
using System.Collections;

public class ActorDirectorSingle : IActorDirector{

	public void AttackActor(IAbilityAOE buff, IActorManager actor)
	{
		actor.BeenAttacted (buff);
	}

	public void AttackActor(float damage, IActorManager actor)
	{
		actor.DecHp(damage);
	}

	public void RecruitSoldier(HeroCamp camp, IActorManager soldier)
	{
		if (soldier.BeenRecruited (camp))
			camp.RecruitSoldier (soldier);
	}

	public void AddActorHp(float hpIncrement, IActorManager actor)
	{
		actor.AddHp (hpIncrement);
	}

	public void DecActorHp(float hpDecrement, IActorManager actor)
	{
		actor.DecHp (hpDecrement);
	}

	public void AddActorMp(float mpIncrement, IActorManager actor)
	{
		actor.AddMp (mpIncrement);
	}

	public void DecActorMp(float mpDecrement, IActorManager actor)
	{
		actor.DecMp (mpDecrement);
	}

	public void ActorDied(IActorManager actor)
	{
		actor.Died ();
	}
}
