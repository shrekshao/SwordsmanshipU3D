using UnityEngine;
using System.Collections;

public class ParticleEndDestroy : MonoBehaviour {

    ParticleSystem ps;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (! ps.IsAlive(false))
        {
            Destroy(this);
            Destroy(gameObject);
        }

    }
}
