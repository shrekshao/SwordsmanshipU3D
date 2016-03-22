using UnityEngine;
using System.Collections;

public class LoseHPBehavior : MonoBehaviour {

    public float moveSpeed;
    public float fadeSpeed;

    private TextMesh textMesh;
    private Camera cameraToLookAt;

    public void Start() {

        //---cache reference to text mesh for changing color---
        textMesh = gameObject.GetComponent< TextMesh >();

        //---cache main camera for billboard feature---
        cameraToLookAt = GameObject.FindGameObjectWithTag( "MainCamera" ).GetComponent< Camera >();
    }
    
    public void Update() {

        //---rise position---
        Vector3 position = gameObject.transform.localPosition;

        gameObject.transform.localPosition = new Vector3(
            position.x, position.y + moveSpeed, position.z
        );
        
        //---fade color---
        Color color = textMesh.color;

        textMesh.color = new Color( color.r, color.g, color.b, color.a - fadeSpeed );

        //---face camera---
        Vector3 v = cameraToLookAt.transform.position - transform.position;

        v.x = v.z = 0.0f;
        transform.LookAt( cameraToLookAt.transform.position - v ); 
        transform.Rotate(0,180,0);
    }
}
