using UnityEngine;
using System.Collections;

public class SpecialSystem : MonoBehaviour {

    SpecialController[] specialController;

	void Start () {
	    specialController = new SpecialController[ 3 ];
        specialController[ 0 ] = GameObject.FindGameObjectWithTag( "Special1Controller" ).GetComponent< SpecialController >();
        specialController[ 1 ] = GameObject.FindGameObjectWithTag( "Special2Controller" ).GetComponent< SpecialController >();
        specialController[ 2 ] = GameObject.FindGameObjectWithTag( "Special3Controller" ).GetComponent< SpecialController >();
	}
	
    public void updateCooldown( int a, float b ) {
        specialController[ a ].coolDownPercent = b;
    }
}
