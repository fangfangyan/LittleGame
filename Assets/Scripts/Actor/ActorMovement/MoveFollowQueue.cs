using UnityEngine;
using System.Collections;

public class MoveFollowQueue : IActorMovement {

	private Vector3 velocity;
	
	private Vector3 destination;

	private float linkedPower;
	
	private UnityEngine.AI.NavMeshAgent agent;
	
	public MoveFollowQueue(UnityEngine.AI.NavMeshAgent agent, float linkedPower)
	{
		this.agent = agent;
		this.linkedPower = linkedPower;
	}
	
	public void SetVelocity(Vector3 velocity)
	{
		this.velocity = velocity;
	}

	public Vector3 GetVelocity()
	{
		return agent.velocity;
	}

	public void SetDestination(Vector3 destination)
	{
		this.destination = destination - (linkedPower * velocity);
	}

	public void SetLinkedPower(float linkedPower)
	{
		this.linkedPower = linkedPower;
	}

	public void ActorUpdate()
	{
		agent.SetDestination(destination);
	}
}
