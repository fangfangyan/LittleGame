using UnityEngine;
using System.Collections;

public interface IHeroState{
	
	void Update (float deltaTime);
	
	void ToNextState(EnumHeroState state);

	void OnTriggerEnter (Collider other);

//	void OnCollisionEnter (Collision other);

	void SetActive(bool active);
	
}
