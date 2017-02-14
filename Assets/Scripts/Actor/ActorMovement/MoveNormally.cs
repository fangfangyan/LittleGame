using UnityEngine;
using System.Collections;

public class MoveNormally : IActorMovement {

	private Vector3 velocity;
	
	private Vector3 destination;
	
	private UnityEngine.AI.NavMeshAgent agent;
	
	public MoveNormally(UnityEngine.AI.NavMeshAgent agent)
	{
		this.agent = agent;
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
		this.destination = destination;
	}
	
	public void ActorUpdate()
	{
////		Vector3 position = actor.transform.localPosition;
////		position += velocity;
//		actor.transform.localPosition += velocity;
////		float delta = Time.deltaTime;
		Vector3 distance = agent.gameObject.transform.localPosition - destination;
		if (distance.x == 0 && distance.z == 0) {
			agent.enabled = false;
			return;
		}
		agent.enabled = true;
		agent.SetDestination(destination);
	}
}
