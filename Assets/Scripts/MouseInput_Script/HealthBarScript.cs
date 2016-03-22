using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
public class HealthBarScript : MonoBehaviour {

	private Camera cameraToLookAt;

    

	public void Start() {
		
		//---cache main camera for billboard feature---
		cameraToLookAt = GameObject.FindGameObjectWithTag( "MainCamera" ).GetComponent< Camera >();
	}


    public void UpdateHPRatio(float ratio)
    {
        transform.localScale = new Vector3(ratio, transform.localScale.y, transform.localScale.z);
    }

	public void Update() {

			
		//---face camera---
		Vector3 v = cameraToLookAt.transform.position - transform.position;

		v.x = v.z = 0.0f;
		transform.LookAt( cameraToLookAt.transform.position - v ); 
		transform.Rotate(0,180,0);
	}
}
}
