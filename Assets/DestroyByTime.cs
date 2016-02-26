using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

    public float life;
        
	void Start () {

        StartCoroutine( destroyByTime() );
	}
	
	IEnumerator destroyByTime() {
        
        yield return new WaitForSeconds( life );
        Destroy( gameObject );
    }
}
