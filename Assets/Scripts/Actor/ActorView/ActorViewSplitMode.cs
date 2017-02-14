using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorViewSplitMode : MonoBehaviour, IActorView, IPoolable {

	private IActorMovement movement;

	private Animator UpperAnimator;

	private Animator LowerAnimator;

	private bool facingRight;

	private bool isActive;

	private Transform targetTransform;

	private Transform actorTransform;

	private Transform armTransform;

	private string animationName;

	private float duration;

	private Utils.TriggerAnimationEnd animationEnd;

	// Use this for initialization
	public void Start()
	{
		UpperAnimator = GetComponent<Animator> ();
		LowerAnimator = transform.GetChild(0).GetComponent<Animator> ();

		facingRight = true;

		actorTransform = transform;
		armTransform = actorTransform.GetChild (1).transform;
	}

	// ActorUpdate is called by state machine
	public void ActorUpdate()
	{
		if (movement == null)
			return;
		if (UpperAnimator == null)
			return;
		UpdateSpeed ();
		UpdateFacing ();
		UpdateShootDirection ();

		if (duration >= 0)
			return;
		if (UpperAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) 
		{
			this.duration = 0;
			PlayAnimationEnd ();
		}
	}

	public void SetMovement(IActorMovement movement)
	{
		this.movement = movement;
	}

	public void SetTarget(Transform target)
	{
		targetTransform = target;
	}

	public void SetActive(bool isActive)
	{
		gameObject.SetActive (isActive);
	}

	private void UpdateSpeed()
	{
		Vector3 v = movement.GetVelocity();
		float speed = Mathf.Pow(new Vector2 (v.x, v.z).SqrMagnitude (), 0.5f);
		bool walk = (speed == 0 ? false : true);
		LowerAnimator.SetBool ("walk", walk);
		LowerAnimator.SetFloat ("speed", speed);
	}

	private void UpdateFacing()
	{
		Vector3 v = movement.GetVelocity();
		if (facingRight && v.x < 0)
			Flip ();
		else if (!facingRight && v.x > 0)
			Flip ();
	}

	private void Flip(){
		Vector3 scale = transform.parent.localScale;
		scale.x *= -1;
		transform.parent.localScale = scale;
		facingRight = !facingRight;
	}

	private void UpdateShootDirection(){
		if (null == targetTransform)
			return;
		Vector3 targetPosition = targetTransform.localPosition;
		Vector3 actorPosition = actorTransform.position;
		Vector2 shootDirection = new Vector2 (targetPosition.x - actorPosition.x
			, targetPosition.z - actorPosition.z);
		Vector2 xAxis = new Vector2 (1f, 0f);
		float angle = Vector2.Angle (xAxis, shootDirection);
		angle = (shootDirection.y > 0 ? angle : -angle);
		if (!facingRight)
			angle = 180f - angle;
		armTransform.localRotation = Quaternion.Euler (0, 0, angle);
	}

	public void Reset()
	{
		LowerAnimator.SetBool ("walk", false);
		LowerAnimator.SetFloat ("speed", 0f);
		armTransform.localRotation = Quaternion.Euler (0, 0, 0);
	}

	public void PlayAnimation(string name, float duration, bool isGlobalMode, Utils.TriggerAnimationEnd animationEnd)
	{
		if (!gameObject.activeSelf)
			return;
		UpperAnimator.ResetTrigger ("UpperNormal");
		UpperAnimator.SetTrigger (name);
		animationName = name;
		this.duration = duration;
		this.animationEnd = animationEnd;

		if(duration > 0)
			Invoke("PlayAnimationEnd", duration);  
	}

	private void PlayAnimationEnd()
	{
		UpperAnimator.ResetTrigger (animationName);
		UpperAnimator.SetTrigger ("UpperNormal");
		if(null != animationEnd)
			animationEnd.Invoke ();
	}
}
