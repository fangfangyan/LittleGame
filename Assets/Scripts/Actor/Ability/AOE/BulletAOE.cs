using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletAOE : MonoBehaviour, IAbilityAOE {

	[SerializeField]
	private AbilityAOEData aoeData;

	private ActorInfo actorInfo;

	private LayerMask targetMask;

	private float duration = 0;

	public void SetAOEInfo(AbilityAOEData data)
	{
		aoeData = data;
	}

	public void SetActorInfo(ActorInfo actorInfo)
	{
		this.actorInfo = actorInfo;
		switch (actorInfo.campType) 
		{
		case ECamp.Creep:
			targetMask = 1 << LayerMask.NameToLayer(ECamp.Friendly.ToString()) 
				| 1 << LayerMask.NameToLayer(ECamp.Enemy.ToString());
			break;
		case ECamp.Enemy:
			targetMask =  1 << LayerMask.NameToLayer(ECamp.Friendly.ToString()) 
				| 1 << LayerMask.NameToLayer(ECamp.Creep.ToString());
			break;
		case ECamp.Friendly:
			targetMask =  1 << LayerMask.NameToLayer(ECamp.Creep.ToString()) 
				| 1 << LayerMask.NameToLayer(ECamp.Enemy.ToString());
			break;
		default:
			targetMask = 0;
			break;
		}
	}

	public void OnActorEnter(GameObject actor)
	{
		if (((1 << actor.layer) & targetMask) == 0)
			return;
		IActorManager actorManager = actor.GetComponent<IActorManager> ();
		actorManager.BeenAttacted(this);
		Destroy (gameObject);
	}

	public ActorBuffType GetBuffType ()
	{
		return aoeData.Type;
	}

	public float GetPower ()
	{
		return aoeData.Power;
	}

	public Transform GetTransform()
	{
		return transform;
	}

//	private void OnCollisionEnter(Collision collision)
//	{
//		GameObject gObj = collision.gameObject;
//		if ((gObj.layer & targetMask) == 0)
//			return;
//		IActorManager actor = gObj.GetComponent<IActorManager> ();
//		actor.DecHp (100f);
//		Destroy (this);
//	}

	private void OnTriggerEnter(Collider other)
	{
		OnActorEnter (other.gameObject);
	}

	// Use this for initialization
	void Start () {
		Rigidbody rigidbody = GetComponent<Rigidbody> ();

		Vector3 force = (transform.rotation * Vector3.right) * aoeData.Speed;

//		rigidbody.velocity = force;
		rigidbody.AddForce (force);
		duration = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (aoeData == null)
			return;
		duration += Time.deltaTime;
		if (duration >= aoeData.Duration)
			Destroy (gameObject);
	}
}
