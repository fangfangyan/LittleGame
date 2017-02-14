using UnityEngine;
using System.Collections;

public class SoldierStateStand : ISoldierState {

	private readonly StatePatternSoldier soldier;
	
//	private SoldierModel soldierModel;
	
	private readonly IActorView actorView;
	
//	private LayerMask layerMask;
	
	public SoldierStateStand(StatePatternSoldier soldier)
	{
		this.soldier = soldier;
//		soldierModel = soldier.GetModel ();
		actorView = soldier.GetActorView();
//		layerMask = Utils.GetPlayerFightMask ();
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
		if (!other.gameObject.CompareTag ("Player"))
			return;
		IActorManager hero = other.gameObject.GetComponent<IActorManager> ();
		hero.GetCamp ().RecruitSoldier (soldier);		
	}
	
	public void ToNextState(EnumSoldierState state)
	{
		SetActive (false);
		soldier.ToNextState (state);
	}
	
	public void SetTarget()
	{
		if (actorView != null)
			actorView.SetTarget (null);
	}
	
	public void SetActive(bool isActive)
	{
		if(isActive)
			SetTarget ();
	}

}
