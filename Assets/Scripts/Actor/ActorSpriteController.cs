using UnityEngine;
using System.Collections;

public class ActorSpriteController : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent agent;
	private Animator animatorWalk;
	private bool facingRight;

	private Transform targetTransform;
	private Transform actorTransform;
	
	// Use this for initialization
	void Start () {
		agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent> ();
		animatorWalk = GetComponentInChildren<Animator> ();
		facingRight = true;

		actorTransform = transform.GetChild (0).transform;
	}

	void LateUpdate () { 
		//SetTargetTransform (agent.transform);
		UpdateSpeed ();
		UpdateFacing ();
		UpdateShootDirection ();
	}

	public void SetTargetTransform(Transform target){
		targetTransform = target;
	}

	private void UpdateSpeed()
	{
		Vector3 v = agent.velocity;
		float speed = Mathf.Pow(new Vector2 (v.x, v.z).SqrMagnitude (), 0.5f);
		bool walk = (speed == 0 ? false : true);
		animatorWalk.SetBool ("walk", walk);
		animatorWalk.SetFloat ("speed", speed);
	}

	private void UpdateFacing()
	{
		Vector3 v = agent.velocity;
		if (facingRight && v.x < 0)
			Flip ();
		else if (!facingRight && v.x > 0)
			Flip ();
	}

	private void Flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		facingRight = !facingRight;
	}
	
	private void UpdateShootDirection(){
		//		if (targetTransform == null)
		//			return;
//		Vector3 targetPosition = targetTransform.localPosition;
		Vector3 actorPosition = actorTransform.position;
		//		Vector2 shootDirection = new Vector2 (targetPosition.x - actorPosition.x
		//		                                      , targetPosition.y - actorPosition.y);
		Vector2 shootDirection = new Vector2 (-1000f - actorPosition.x
		                                      , -1000f - actorPosition.z);
		Vector2 xAxis = new Vector2 (1f, 0f);
		float angle = Vector2.Angle (xAxis, shootDirection);
		angle = (shootDirection.y > 0 ? angle : -angle);
		if (!facingRight)
			angle = 180f - angle;
		actorTransform.localRotation = Quaternion.Euler (0, 0, angle);
	}

}
