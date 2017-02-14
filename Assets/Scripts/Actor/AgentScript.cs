using UnityEngine;
using System.Collections;

public class AgentScript : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent agent;

	// Use this for initialization
	void Start () 
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				//maybe the property of tag is not exsist in unity v5.2+
				if(hit.collider.tag == "Ground")
				{
					agent.SetDestination(hit.point);
				}
			}

		}

	}
}
