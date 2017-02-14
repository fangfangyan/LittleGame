using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Comparer<T> : IComparer<T> where T : ISortable {

	private static Comparer<T> instance;

	public static Comparer<T> GetInstance()
	{
		if (null == instance)
			instance = new Comparer<T> ();
		return instance;
	}

	public int Compare (T object1, T object2)
	{
		return object1.GetPower () - object2.GetPower ();
	}
}
