using UnityEngine;
using System.Collections;

public interface IActorView{

	void ActorUpdate();

	void SetMovement(IActorMovement movement);

	void SetTarget(Transform target);

	void SetActive(bool isActive);

	void PlayAnimation(string name, float duration, bool isWholeMode, Utils.TriggerAnimationEnd animationEnd);

}
