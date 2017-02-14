using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStateDead : ISoldierState {

	private readonly StatePatternSoldier soldier;

	private readonly IActorView actorView;

	public SoldierStateDead(StatePatternSoldier soldier)
	{
		this.soldier = soldier;
		actorView = soldier.GetActorView();
	}

	public void Update (float deltaTime)
	{	
		UpdateView ();
	}

	public void UpdateView()
	{
		actorView.ActorUpdate ();
	}

	public void OnTriggerEnter (Collider other)
	{

	}

	public void ToNextState(EnumSoldierState state)
	{
		SetActive (false);
		soldier.ToNextState (state);
	}

	public void SetActive(bool isActive)
	{
		if(isActive)
			actorView.PlayAnimation ("HeroDead", -1f, true, TriggerAnimationEnd);
	}

	void TriggerAnimationEnd()
	{
		soldier.Destroy ();
	}
}
