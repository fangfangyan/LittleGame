using UnityEngine;
using System.Collections;

public interface ICreepState {

	void Update (float deltaTime);
	
	void ToNextState(EnumCreepState state);
	
	void OnTriggerEnter (Collider other);
	
	//	void OnCollisionEnter (Collision other);
	
	void SetActive(bool active);

}
