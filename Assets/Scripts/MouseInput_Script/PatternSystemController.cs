using UnityEngine;
using System.Collections;

public class PatternSystemController : MonoBehaviour {
    
    public GameObject prompt;
    public float interval;
    public float nextTime;
    public bool[] isTouched;

    private bool isDrawing;
    private int nPrompts;
    private GameObject[] prompts;
    private Swordsmanship.SwordsmanControl swordsmanshipController;

    private float[] pattern1 = {
        10, 10, 
        -10, 10, 
        -10, -10, 
        10, -10, 
    };

    private float[] pattern2 = {
        25, -10, 
        -25, 10, 
        -25, -10, 
        25, 10, 
        0, -15, 
        0, 15, 
    };

    private float[] pattern3 = {
        0, 0, 
        0, 20, 
        22, 11, 
        22, -11,
        0, -20, 
        -22, -11, 
        -22, 11, 
    };

	void Start () {

        //---swordsman controller---
        swordsmanshipController = GameObject.FindGameObjectWithTag( "Player" ).GetComponent< Swordsmanship.SwordsmanControl >();

        isDrawing = false;
        prompts = new GameObject[ 10 ];
        isTouched = new bool[ 10 ];
	}
	
    void createPattern( int a, float[] b ) {
        
        nPrompts = a;
        nextTime = Time.time + interval;

        //---generate prompts---
        for( int i = 0; i < a; ++i ) {
            
            Vector3 position = new Vector3( b[ i << 1 ], b[ i << 1 | 1 ], 0 );

            //---initialize prompt---
            prompts[ i ] = Instantiate( prompt, new Vector3(), new Quaternion() ) as GameObject;
            prompts[ i ].transform.parent = gameObject.transform;
            prompts[ i ].transform.localPosition = position;
            prompts[ i ].transform.localRotation = new Quaternion();
            prompts[ i ].GetComponent< PromptAttributes >().setID( i );
            isTouched[ i ] = false;
        }
    }

    void ClearPattern() {
        
        //---destroy prompts---
        for( int i = 0; i < nPrompts; ++i ) {

            //---destroy prompt---
            if( prompts[ i ] ) Destroy( prompts[ i ] );
        }
    }

    public void performSpecial( int a ) {

        switch( a ) {
            case 0:
                ClearPattern();
                isDrawing = true;
                createPattern( 4, pattern1 );
                break;
            case 1:
                ClearPattern();
                isDrawing = true;
                createPattern( 6, pattern2 );
                break;
            case 2:
                ClearPattern();
                isDrawing = true;
                createPattern( 7, pattern3 );
                break;
        }
    }

	void Update () {
        
        //---generate pattern---
        //if( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
        //    ClearPattern();
        //    isDrawing = true;
        //    createPattern( 4, pattern1 );
        //}
        //if( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
        //    ClearPattern();
        //    isDrawing = true;
        //    createPattern( 6, pattern2 );
        //}
        //if( Input.GetKeyDown( KeyCode.Alpha3 ) ) {
        //    ClearPattern();
        //    isDrawing = true;
        //    createPattern( 7, pattern3 );
        //}
        
        //---check touch order---
        if( isDrawing ) {
            if( Time.time > nextTime ) {
                ClearPattern();
                isDrawing = false;
                //Debug.Log( "stop()" );
                swordsmanshipController.SpecialMoveStop();
            } else {
                for( int i = 1; i < nPrompts; ++i ) {
                    if( isTouched[ i ] && !isTouched[ i - 1 ] ) {
                        ClearPattern();
                        isDrawing = false;
                        //Debug.Log( "stop()" );
                        swordsmanshipController.SpecialMoveStop();
                    }
                }
            }
        }
	}
}
