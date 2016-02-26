using UnityEngine;
using System.Collections;

public class Special1Behavior : MonoBehaviour {

    public GameObject explosion;
    public int nExplosions;
    public float interval;
    public float distance;
    
	void Start () {
	    
        StartCoroutine( spawnExplosion() );
	}
	
	IEnumerator spawnExplosion() {

        for( int i = 1; i <= nExplosions; ++i ) {
            Instantiate( explosion, transform.position + transform.forward * distance * i, new Quaternion() );
            yield return new WaitForSeconds( interval );
        }
    }
}
