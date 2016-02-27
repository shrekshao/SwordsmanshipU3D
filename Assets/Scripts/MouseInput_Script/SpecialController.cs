using UnityEngine;
using System.Collections;

public class SpecialController : MonoBehaviour {

    public float coolDownPercent;
    public Transform iconTransform;

    private float coolDownStart;
    //private bool isActive;

	void Start () {
	    //isActive = true;
	}
	
	void Update () {
	    
        iconTransform.localPosition = new Vector3(0, 0, coolDownPercent );
        //if( !isActive ) {

        //    iconTransform.localPosition = new Vector3( 0, 0, ( Time.time - coolDownStart ) / coolDown );
        //    if( Time.time > coolDownStart + coolDown ) isActive = true;
        //}
	}

    //bool use() {

    //    if( !isActive ) return false;

    //    isActive = false;
    //    coolDownStart = Time.time;
        
    //    return true;
    //}
}
