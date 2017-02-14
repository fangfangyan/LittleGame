using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveFollowHero : IActorMovement {

	private Vector3 velocity;

	private Vector3 nextFrameVelocity;
	
	private Vector3 destination;

	private Vector3 distance;

	private IActorManager actor;
	
	private GameObject gameObject;

	private GameObject prev;

	private IActorMovement prevMovement;

	private HeroCamp camp;

	private LinkedList<Vector3> prevLocations;

	private LinkedList<Vector3> preVelocities;

	public MoveFollowHero(IActorManager actor)
	{
		this.actor = actor;
		gameObject = actor.GetGameObject ();
		camp = actor.GetCamp ();
		prevLocations = new LinkedList<Vector3> ();
		preVelocities = new LinkedList<Vector3> ();
//		this.destination = gameObject
	}

	public void SetVelocity(Vector3 velocity)
	{
//		this.velocity = velocity;
	}

	public Vector3 GetVelocity()
	{
		return velocity;
	}

	public void SetDestination(Vector3 destination)
	{
//		this.destination = destination;
	}

	public void ActorUpdate()
	{
		IActorManager prevActor = actor.GetPrev ();

		if (prev != prevActor.GetGameObject ()) {
			prev = prevActor.GetGameObject ();
			prevMovement = prevActor.GetMovement ();
		}

		Vector3 velocityPrev = prevMovement.GetVelocity ();

		if (velocityPrev.magnitude != 0) 
		{
			preVelocities.AddLast (velocityPrev);
			prevLocations.AddLast (prev.transform.localPosition);
		}

		float distance = camp.GetLinkedDistance (actor);
		
//		Vector3 deltaDistance;
//
//		if (velocityPrev.sqrMagnitude > 0) {
//			nextFrameVelocity = velocityPrev;
//			deltaDistance = nextFrameVelocity * distance;
//		} else {
//			nextFrameVelocity = velocity.normalized * 2;
//			deltaDistance = nextFrameVelocity * distance;
//		}

		if ((gameObject.transform.localPosition - prev.transform.localPosition).magnitude <= distance) {
			velocity =  Vector3.zero;
			return;
		}

		if (preVelocities.Count <= 0)
			return;

//		if (velocity.magnitude == 0)
//			return;

//		if (velocityPrev.sqrMagnitude < 1) {
//
//			gameObject.transform.localPosition = prev.transform.localPosition - deltaDistance;
//		} else {
		velocity = preVelocities.First.Value;
		preVelocities.RemoveFirst ();
		gameObject.transform.localPosition = prevLocations.First.Value;
		prevLocations.RemoveFirst ();
//		}


	}
}
