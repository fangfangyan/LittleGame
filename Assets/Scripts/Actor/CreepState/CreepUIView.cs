using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepUIView : MonoBehaviour {

	private CreepModel actor;

	private float hpMax;

	private Transform bloodTransform;

	// Use this for initialization
	void Start () {
		actor = GetComponentInParent<StatePatternCreep> ().GetModel ();
		hpMax = actor.creepData.MaxHp;
		bloodTransform = transform.GetChild (0);
	}

	// Update is called once per frame
	void Update () {
		float hp = actor.hp;
		bloodTransform.localScale = new Vector3(hp / hpMax, bloodTransform.localScale.y, bloodTransform.localScale.z);
		bloodTransform.localPosition = new Vector3 (hp / hpMax - 1, bloodTransform.localPosition.y, bloodTransform.localPosition.z);
	}
}
