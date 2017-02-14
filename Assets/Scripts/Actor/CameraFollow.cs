using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	[SerializeField]
	private float xMin;
	
	[SerializeField]
	private float xMax;
	
	[SerializeField]
	private float zMin;
	
	[SerializeField]
	private float zMax;
	
	private Transform target;

	// Use this for initialization
	void Start () 
	{
		Camera camera = this.GetComponent<Camera> ();
		float halfCameraH = camera.orthographicSize;
		float halfCameraW = halfCameraH * camera.aspect;

		xMin += halfCameraW;
		xMax -= halfCameraW;
		zMin += halfCameraH;
		zMax -= halfCameraH;
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	void LateUpdate() 
	{
		if (null == target)
			return;
		transform.position = new Vector3 (Mathf.Clamp (target.localPosition.x, xMin, xMax)
		                            , transform.localPosition.y
		                                  , Mathf.Clamp (target.localPosition.z, zMin, zMax));
	}

	public void SetTarget(GameObject hero)
	{
		target = hero.transform;
	}
}
