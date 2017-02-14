using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroCamp {

	/// <summary>
	/// The head of this camp.
	/// </summary>
	private IActorManager hero;

	private IActorManager tail;

	private ECamp campType;

	public readonly int CampId;

	public HeroCamp(int campId, IActorManager hero)
	{
		CampId = campId;
		this.hero = hero;
		this.tail = hero;
	}

	/// <summary>
	/// Recruits the soldier.
	/// </summary>
//  ///<returns><c>true</c>, If the recruitment succeeded, <c>false</c> otherwise.</returns>
	/// <param name="soldier">Soldier.</param>
	public void RecruitSoldier(IActorManager soldier){
		if (!soldier.BeenRecruited (this))
			return;
		soldier.SetPrev (tail);
		tail.SetNext (soldier);
		tail = soldier;
	}

	public void RemoveSoldier(IActorManager soldier)
	{
		if (null == soldier.GetNext ()) 
		{
			soldier.GetPrev ().SetNext (null);
			tail = soldier.GetPrev ();
			return;
		}
		soldier.GetNext ().SetPrev (soldier.GetPrev ());
		soldier.GetPrev ().SetNext (soldier.GetNext ());
	}

	/// <summary>
	/// Sets the type of the camp.
	/// </summary>
	/// <param name="campType">Camp type.</param>
	public void SetCampType(ECamp campType)
	{
		this.campType = campType;
	}

	public ECamp GetCampType()
	{
		return campType;
	}
	/// <summary>
	/// Gets the actor by identifier.
	/// </summary>
	/// <returns>The actor by identifier.</returns>
	public IActorManager GetActorById(string id)
	{
		IActorManager actor = hero;
		while (actor != null) 
		{
			if(string.Equals(actor.GetActorId(), id))
				return actor;
		}
		return null;
	}

	public IActorManager GetHero()
	{
		return hero;
	}

	public int GetIndex(IActorManager actor)
	{
		IActorManager current = hero;
		int count = 0;
		while (current != actor) 
		{
			current = current.GetNext();
			count++;
		}
		return count;
	}

	public float GetLinkedDistance(IActorManager actor)
	{
//		int index = GetIndex (actor);
//		return index * 1f;
		return 0.5f;
	}

	public int Count()
	{
		IActorManager actor = hero;
		int count = 1;
		while (actor != tail) 
		{
			actor = actor.GetNext();
			count++;
		}
		return count;
	}
	
}
