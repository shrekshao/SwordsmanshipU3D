using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

	private Camera cameraToLookAt;

	public void Start() {
		
		//---cache main camera for billboard feature---
		cameraToLookAt = GameObject.FindGameObjectWithTag( "MainCamera" ).GetComponent< Camera >();
	}

	public void Update() {

			
		//---face camera---
		Vector3 v = cameraToLookAt.transform.position - transform.position;

		v.x = v.z = 0.0f;
		transform.LookAt( cameraToLookAt.transform.position - v ); 
		transform.Rotate(0,180,0);
	}
}
