using UnityEngine;
using System.Collections;

public class ActorViewBullet : MonoBehaviour, IActorView, IPoolable {
	
	private IActorMovement movement;

	private Animator animatorUpper;

	private Animator animatorDown;

	private Animator animatorWhole;

	private bool facingRight;

	private bool isActive;
	
	private Transform targetTransform;

	private Transform actorTransform;

	private Transform armTransform;

	// Use this for initialization
	public void Start()
	{
		animatorUpper = GetComponentInChildren<Animator> ();
		facingRight = true;
		
		actorTransform= transform.GetChild (0).transform;
		armTransform = actorTransform.GetChild (1).transform;
	}

	// ActorUpdate is called by state machine
	public void ActorUpdate()
	{
		if (movement == null)
			return;
		UpdateSpeed ();
		UpdateFacing ();
		UpdateShootDirection ();
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
		this.isActive = isActive;
	}
	
	private void UpdateSpeed()
	{
		Vector3 v = movement.GetVelocity();
		float speed = Mathf.Pow(new Vector2 (v.x, v.z).SqrMagnitude (), 0.5f);
		bool walk = (speed == 0 ? false : true);
		animatorUpper.SetBool ("walk", walk);
		animatorUpper.SetFloat ("speed", speed);
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
		animatorUpper.SetBool ("walk", false);
		animatorUpper.SetFloat ("speed", 0f);
		actorTransform.localRotation = Quaternion.Euler (0, 0, 0);
	}

	public void PlayAnimation(string name, float duration, bool isGlobalMode, Utils.TriggerAnimationEnd animationEnd)
	{
		
	}
}

