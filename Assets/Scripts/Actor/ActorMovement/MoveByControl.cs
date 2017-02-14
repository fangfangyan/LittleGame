using UnityEngine;
using System.Collections;

public class MoveByControl : IActorMovement {

//	private Vector3 velocity;

	private Vector3 destination;

	private UnityEngine.AI.NavMeshAgent agent;

	public MoveByControl(UnityEngine.AI.NavMeshAgent agent)
	{
		this.agent = agent;
	}

	public void SetVelocity(Vector3 velocity)
	{
//		agent.velocity = velocity;
	}

	public Vector3 GetVelocity()
	{
		return agent.velocity;
	}

	public void SetDestination(Vector3 destination)
	{
		this.destination = destination;
	}
	
	public void ActorUpdate()
	{
		Vector3 distance = agent.gameObject.transform.localPosition - destination;
		if (distance.x == 0 && distance.z == 0) {
			agent.enabled = false;
			return;
		}
		agent.enabled = true;
		agent.SetDestination(destination);
	}

}
