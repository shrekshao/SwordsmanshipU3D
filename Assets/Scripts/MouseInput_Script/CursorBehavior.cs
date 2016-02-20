using UnityEngine;
using System.Collections;

public class CursorBehavior : MonoBehaviour {
    
    private Rigidbody rb;
    private float scaler;

    void Start() {

        //---cache rigidbody reference---
        rb = GetComponent<Rigidbody>();

        scaler = 1.0f / 10.0f;
    }

    void Update() {

    }

    void FixedUpdate() {
        
        //---move cursor---
        //float mouseX = Input.GetAxis( "Mouse X" );
        //float mouseY = Input.GetAxis( "Mouse Y" );
        //Vector3 force = new Vector3( mouseX, mouseY, 0 );

        //rb.AddForce( force * 1000 );

        Vector3 mousePosition = Input.mousePosition;

        mousePosition -= new Vector3( Screen.width * 0.5f, Screen.height * 0.5f, 0.0f );
        transform.localPosition = mousePosition * scaler;
    }
    
}
