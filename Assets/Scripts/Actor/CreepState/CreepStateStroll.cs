using UnityEngine;
using System.Collections;

public class CreepStateStroll : ICreepState {

	private readonly StatePatternCreep creep;
	
	private CreepModel creepModel;
	
	private readonly IActorView actorView;
	
	private readonly IActorMovement movement;
	
	private LayerMask layerMask;

	private readonly Vector3[] strollDestination;

	private readonly float strollDuration;

	private float NextDestinationTime;
	
	private int currentDestination;
	
	public CreepStateStroll(StatePatternCreep creep)
	{
		this.creep = creep;
		creepModel = creep.GetModel ();
		actorView = creep.GetActorView();
		movement = creep.GetMovement ();
		layerMask = Utils.GetCreepFightMask ();

		strollDuration = 3f;
		currentDestination = 0;
		NextDestinationTime = -1f;
		strollDestination = new Vector3[4];
		strollDestination [0] = new Vector3 (3, 0, 0);
		strollDestination [1] = new Vector3 (0, 0, -3);
		strollDestination [2] = new Vector3 (-3, 0, 0);
		strollDestination [3] = new Vector3 (0, 0, 3);
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
		if (NextDestinationTime < 0) 
		{
			NextDestinationTime = Time.time + strollDuration;
			Vector3 distance = strollDestination[currentDestination];
			distance += creep.transform.localPosition;
			movement.SetDestination(distance);
			currentDestination++;
			currentDestination %= strollDestination.Length;
		}
		else if(Time.time >= NextDestinationTime)
		{
			NextDestinationTime = -1;
		}

		movement.ActorUpdate ();
	}
	
	public void UpdateState()
	{	
		if (FindEnimy ()) 
		{
			ToNextState (EnumCreepState.Alert);
			return;
		}
	}
	
	private bool FindEnimy()
	{
		
		RaycastHit objhit;
		bool hasEnimy = Utils.SphereCast (creep.transform.localPosition, 
		                                  creepModel.DetectRange, 
		                                  creep.transform.forward, 
		                                  out objhit,
		                                  layerMask);
		return hasEnimy;
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
