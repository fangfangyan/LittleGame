using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils {

	public delegate void TriggerEffectDelegate(AbilityEffectData effectInfo);

	public delegate void TriggerAnimationEnd();

	private static LayerMask actorMask =  1 << 9 | 1 << 10 | 1 << 11 | 1 << 12;

	public static LayerMask GetMaskByTypes(List<ECamp> camps)
	{
		LayerMask mask = 0;

		foreach (ECamp camp in camps) 
			mask = mask ^ 1 << LayerMask.NameToLayer (camp.ToString ());

		return mask;
	}

	public static LayerMask GetActorMask()
	{
		return actorMask;
	}

	public static LayerMask GetPlayerFightMask()
	{
//		LayerMask maskAll = 0xffff;
		LayerMask mask = 1 << 9 | 1 << 12;
//		mask = mask ^ maskAll;
		return mask;
	}

	public static LayerMask GetCreepFightMask()
	{
		//		LayerMask maskAll = 0xffff;
		LayerMask mask = 1 << 9 | 1 << 10;
		//		mask = mask ^ maskAll;
		return mask;
	}


	public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, int layerMask)
	{
		int times = Mathf.FloorToInt(radius * 2 * Mathf.PI);
		float angle = 360f / times;

		if (Physics.Raycast (origin, direction, out hitInfo, radius, layerMask))
			return true;

		for (int i = 1; i < times; i++) 
		{
			Vector3 iDirection = Quaternion.Euler (0, angle * i, 0) * direction;
			if (Physics.Raycast (origin, iDirection, out hitInfo, radius, layerMask))
				return true;
		}
		return false;
	}

	public static Transform GetChildByName(Transform parent, string name)
	{
		Transform child;
		foreach (Transform transform in parent) 
		{
			if(transform.name.Equals(name))
				return transform;

			child = GetChildByName(transform, name);

			if(null != child)
				return child;
		}
		return null;
	}

	public static LayerMask GetAoeLayer()
	{
		return LayerMask.NameToLayer ("Aoe");
	}

	public static LayerMask GetLayerByType(ECamp camp)
	{
		return LayerMask.NameToLayer (camp.ToString ());
	}

//	public static string GetLayerByCamp(ECamp camp)
//	{
//		switch(camp)
//		{
//		case ECamp.Creep:
//			return "";
//		case ECamp.Enemy:
//			return "";
//		case ECamp.Friendly:
//			return "";
//		case ECamp.Soldier:
//			return "";
//		}
//	}
}
