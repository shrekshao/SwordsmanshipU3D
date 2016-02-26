using UnityEngine;
using System.Collections;

public class PromptAttributes : MonoBehaviour {

    public int id;

    private ParticleSystem ps;
    private PatternSystemController controller;

    public void Start() {
        ps = gameObject.GetComponentInChildren< ParticleSystem >();
        if( ps.isPlaying ) ps.Stop();
        controller = GameObject.FindGameObjectWithTag( "Pattern System Controller" ).GetComponent< PatternSystemController >();
    }

    public void OnTriggerEnter(Collider other) {

        //---touched by cursor---
        if(other.tag != "Cursor") return;
        if( !controller.isTouched[ 0 ] && id != 0 ) return;

        controller.isTouched[id] = true;
        if( !ps.isPlaying ) ps.Play();
    }

    public void OnTriggerExit(Collider other) {

        //---same as enter---
        OnTriggerEnter(other);
    }

    public void setID( int _id ) {
        id = _id;
        gameObject.GetComponentInChildren< TextMesh >().text = id + 1 + "";
    }
}
