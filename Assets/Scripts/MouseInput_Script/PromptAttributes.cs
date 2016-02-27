using UnityEngine;
using System.Collections;

public class PromptAttributes : MonoBehaviour {

    public int id;

    private ParticleSystem ps;
    private PatternSystemController controller;
    private Swordsmanship.SwordsmanControl swordsmanshipController;

    public void Start() {

        //---swordsman controller---
        swordsmanshipController = GameObject.FindGameObjectWithTag( "Player" ).GetComponent< Swordsmanship.SwordsmanControl >();

        ps = gameObject.GetComponentInChildren< ParticleSystem >();
        if( ps.isPlaying ) ps.Stop();
        controller = GameObject.FindGameObjectWithTag( "PatternSystemController" ).GetComponent< PatternSystemController >();
    }

    public void OnTriggerEnter(Collider other) {

        //---touched by cursor---
        if(other.tag != "Cursor") return;

        //---first touch must be patter 1---
        if( !controller.isTouched[ 0 ] && id != 0 ) return;

        //Debug.Log( "nextState()" );
        swordsmanshipController.SpecialMoveNextStage();
        controller.isTouched[id] = true;
        controller.nextTime = Time.time + controller.interval;
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
