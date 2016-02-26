using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {

    public float speed;
    
    void FixedUpdate() {
        
        transform.position = transform.position + transform.forward * speed;
    }
}
