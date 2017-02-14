using UnityEngine;
using System.Collections;

public class SoldierStateStroll : ISoldierState {

	private readonly StatePatternSoldier soldier;
	
	private SoldierModel soldierModel;
	
	private readonly IActorView actorView;
	
	private IActorMovement movement;
	
	private LayerMask layerMask;

	public SoldierStateStroll(StatePatternSoldier soldier)
	{
		this.soldier = soldier;
		soldierModel = soldier.GetModel ();
		actorView = soldier.GetActorView();
		movement = soldier.GetMovement ();
		layerMask = Utils.GetPlayerFightMask ();
	}
	
	public void Update (float deltaTime)
	{
		UpdateMovement ();		
		UpdateView ();
		UpdateAbility (deltaTime);
		UpdateState ();
	}
	
	public void UpdateView()
	{
		actorView.ActorUpdate ();
	}
	
	public void UpdateAbility(float deltaTime)
	{
		if (null == soldierModel.activeAbilities)
			return;
		foreach (IActiveAbility ability in soldierModel.activeAbilities)
			ability.Update (deltaTime);
	}
	
	public void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Enemy"))
			ToNextState (EnumSoldierState.Fight);
		
	}

	public void UpdateMovement()
	{	 
		movement.ActorUpdate ();
	}
	
	public void UpdateState()
	{	
		if (FindEnimy ()) 
		{
			ToNextState (EnumSoldierState.Fight);
			return;
		}
	}
	
	private bool FindEnimy()
	{
		
		RaycastHit objhit;
		bool hasEnimy = Utils.SphereCast (soldier.transform.localPosition, 
		                                  soldierModel.DetectRange, 
		                                  soldier.transform.forward, 
		                                  out objhit,
		                                  layerMask);
		return hasEnimy;
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
		if (isActive) {
			SetTarget ();
			movement = soldier.GetMovement ();
		}
	}

}
