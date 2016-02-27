using UnityEngine;
using System.Collections;

public class Special2Behavior : MonoBehaviour {

    public GameObject flame;
    public int nFlames;
    public float interval;
    
    public void Start() {

        StartCoroutine( spawnFlame() );
    }

    IEnumerator spawnFlame() {

        float[] angle = { 20, -20, 90 };

        for(int i = 0; i < nFlames; ++i) {
            yield return new WaitForSeconds(interval);
            Instantiate(flame, transform.position, Quaternion.Euler( new Vector3( 0, 0, angle[ i ] ) ) );
        }
    }
}
