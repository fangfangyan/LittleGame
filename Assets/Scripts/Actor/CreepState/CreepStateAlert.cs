using UnityEngine;
using System.Collections;

public class CreepStateAlert : ICreepState {

	public static readonly float LimitTimeToStroll = 10f;

	private readonly StatePatternCreep creep;
	
	private CreepModel creepModel;
	
	private readonly IActorView actorView;
	
	private readonly IActorMovement movement;

	private GameObject targetActor;
	
	private LayerMask layerMask;

	private float timeToStroll;
	
	public CreepStateAlert(StatePatternCreep creep)
	{
		this.creep = creep;
		creepModel = creep.GetModel ();
		actorView = creep.GetActorView();
		movement = creep.GetMovement ();
		layerMask = Utils.GetCreepFightMask ();
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
		if (null == creepModel.activeAbilities)
			return;
		foreach (IActiveAbility ability in creepModel.activeAbilities)
			ability.Update (deltaTime);
	}
	
	public void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player"))
			ToNextState (EnumCreepState.Fight);		
	}
	
	public void UpdateMovement()
	{
		if (null != targetActor)
			movement.SetDestination (targetActor.transform.localPosition);
		movement.ActorUpdate ();
	}
	
	public void UpdateState()
	{	
		if (targetActor != null && targetActor.activeSelf) 
		{
			Vector3 distance = creep.transform.localPosition - targetActor.transform.localPosition;
			if(distance.magnitude < creepModel.FightRange)
			{
				ToNextState (EnumCreepState.Fight);
				return;
			}
			else if(distance.magnitude < creepModel.DetectRange)
			{
				timeToStroll = -1;
				return;
			}
		}

		if (FindEnimy ()) {
			return;
		} 

		if (timeToStroll < 0) 
			timeToStroll = Time.time + LimitTimeToStroll;
		else if(Time.time >= timeToStroll)
			ToNextState(EnumCreepState.Stroll);

	}
	
	private bool FindEnimy()
	{	
		RaycastHit objhit;

		targetActor = null;

		if (Utils.SphereCast (creep.transform.localPosition, 
		                     creepModel.DetectRange, 
		                     creep.transform.forward, 
		                     out objhit,
		                     layerMask)) 
		{
			targetActor = objhit.collider.gameObject;
			return true;
		}

		return false;
	}
	
	public void ToNextState(EnumCreepState state)
	{
		SetActive (false);
		creep.ToNextState (state);
	}
	
	public void SetTarget()
	{
		if (actorView != null)
			actorView.SetTarget (null);
	}
	
	public void SetActive(bool isActive)
	{
		if (!isActive)
			return;
		SetTarget ();
	}
}
