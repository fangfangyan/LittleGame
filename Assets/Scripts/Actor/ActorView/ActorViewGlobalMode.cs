using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorViewGlobalMode : MonoBehaviour, IActorView, IPoolable {

//	private IActorMovement movement;

	private Animator globalAnimator;

	private Transform armTransform;

	private string animationName;

	private Utils.TriggerAnimationEnd animationEnd;

	private float duration;

	// Use this for initialization
	public void Start()
	{
		globalAnimator = GetComponentInChildren<Animator> ();
	}

	public void ActorUpdate()
	{
		if (duration >= 0)
			return;
		if (globalAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) 
		{
			this.duration = 0;
			PlayAnimationEnd ();
		}
			
	}

	public void SetMovement(IActorMovement movement)
	{
//		this.movement = movement;
	}

	public void SetTarget(Transform target)
	{

	}

	public void SetActive(bool isActive)
	{
		gameObject.SetActive (isActive);
	}

	public void Reset()
	{

	}

	public void PlayAnimation(string name, float duration, bool isGlobalMode, Utils.TriggerAnimationEnd animationEnd)
	{
		if (!gameObject.activeSelf)
			return;

		if(globalAnimator == null)
			globalAnimator = GetComponentInChildren<Animator> ();
		
		globalAnimator.ResetTrigger ("HeroNormal");
		globalAnimator.SetTrigger (name);

		this.animationName = name;
		this.duration = duration;
		this.animationEnd = animationEnd;

		if (duration > 0)
			Invoke ("PlayAnimationEnd", duration);
	}

	private void PlayAnimationEnd()
	{
		this.duration = 0;
		globalAnimator.ResetTrigger (animationName);
		globalAnimator.SetTrigger ("HeroNormal");
		if(null != animationEnd)
			animationEnd.Invoke ();
	}
}
