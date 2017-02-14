using UnityEngine;
using System.Collections;

public interface ISoldierState {
		
	void Update (float deltaTime);
	
	void ToNextState(EnumSoldierState state);
	
	void OnTriggerEnter (Collider other);
	
	//	void OnCollisionEnter (Collision other);
	
	void SetActive(bool active);
		
}
