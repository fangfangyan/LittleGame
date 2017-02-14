using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorView : MonoBehaviour, IActorView, IPoolable {

	private IActorView actorViewGlobalMode;
	 
	private IActorView actorViewSplitMode;

	private IActorView currentMode;

	private Utils.TriggerAnimationEnd animationEnd;

	// Use this for initialization
	public void Start()
	{		
		if (currentMode == null)
			init ();
	}

	public void ActorUpdate()
	{
		currentMode.ActorUpdate ();
	}

	public void SetMovement(IActorMovement movement)
	{
		if (currentMode == null)
			init ();
		actorViewGlobalMode.SetMovement (movement);
		actorViewSplitMode.SetMovement (movement);
	}

	public void SetTarget(Transform target)
	{
		if (currentMode == null)
			init ();
		actorViewGlobalMode.SetTarget (target);
		actorViewSplitMode.SetTarget (target);
	}

	public void SetActive(bool isActive)
	{
//		this.isActive = isActive;
	}

	public void Reset()
	{
		currentMode = actorViewGlobalMode;
	}

	public void PlayAnimation(string name, float duration, bool isGlobalMode, Utils.TriggerAnimationEnd animationEnd)
	{
		if (currentMode == null)
			init ();
		
		currentMode.SetActive (false);

		if (isGlobalMode) 
			currentMode = actorViewGlobalMode;
		else
			currentMode = actorViewSplitMode;

		currentMode.SetActive (true);

		this.animationEnd = animationEnd;

		currentMode.PlayAnimation (name, duration, isGlobalMode, TriggerAnimationEnd);
	}

	void TriggerAnimationEnd()
	{
		currentMode.SetActive (false);
		currentMode = actorViewSplitMode;
		currentMode.SetActive (true);
		if(null != animationEnd)
			animationEnd.Invoke ();
	}

	private void init()
	{
		GameObject globalMode = transform.GetChild (0).gameObject;
		GameObject splitMode = transform.GetChild (1).gameObject;
		actorViewGlobalMode = globalMode.GetComponent<IActorView> ();
		actorViewSplitMode = splitMode.GetComponent<IActorView> ();	
		actorViewGlobalMode.SetActive (false);
		actorViewSplitMode.SetActive (true);
		currentMode = actorViewSplitMode;
	}
}
