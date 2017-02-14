using UnityEngine;
using System.Collections;

public class ActorManager {

	private static ActorManager instance;

	private ActorFactory factory;
	
	private ActorManager()
	{
		factory = ActorFactory.GetInstance ();
	}
	
	public static ActorManager GetInstance()
	{
		if (null == instance)
			instance = new ActorManager ();
		return instance;
	}

	public GameObject CreateHero()
	{
		return factory.CreateHero ();
	}

	public GameObject CreatSoldier()
	{
		return factory.CreateSoldier ();
	}

	public GameObject CreateCreep()
	{
		return factory.CreateCreep ();
	}

	public IAbilityAOE CreateAOE(string name, ActorInfo actorInfo, Transform goalTransform)
	{
		IAbilityAOE aoe = factory.CreateAOE (name);
		aoe.SetActorInfo (actorInfo);
		Transform transform = aoe.GetTransform ();
		transform.localPosition = goalTransform.position;

		Vector3 angle = goalTransform.rotation.eulerAngles;
		if (goalTransform.lossyScale.x < 0 || goalTransform.lossyScale.y < 0) 
			angle = new Vector3 (angle.x, 180f + angle.y, angle.z);
		angle += new Vector3 (0, transform.localRotation.eulerAngles.y, 0);
		transform.localRotation = Quaternion.Euler (angle);
		return aoe;
	}
}
