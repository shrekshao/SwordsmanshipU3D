using UnityEngine;
using System.Collections;

using Swordsmanship;

public class MouseBehavior : MonoBehaviour {
    
    public int statusLMB;
    public int statusRMB;
    public int prepareResponseTime;
    public float attackResponseSpeed;
    public float blockResponseSpeed;

    private float dirX;
    private float dirY;
    private int prepareDelay;

    public delegate void MouseInputDelegate(Swordsmanship.MouseInputStruct m_input);
    public static event MouseInputDelegate mouseInputDelegate;

    void Start() {

        //---lock cursor---
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        statusLMB = 0;


        //register delegate
        
    }

    void Update() {

        if( Input.GetKeyDown( "mouse 0" ) ) { statusLMB = 1; prepareDelay = prepareResponseTime; }
        if (Input.GetKeyUp("mouse 0")) {
            statusLMB = 0;
            Debug.Log("Release");
            mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Idle));
        }
        if( Input.GetKeyDown( "mouse 1" ) ) statusRMB = 1;
        if( Input.GetKeyUp( "mouse 1" ) ) { mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Block_Exit)); statusRMB = 0; }

        float moveX = Input.GetAxis( "Mouse X" );
        float moveY = Input.GetAxis( "Mouse Y" );


        //---attack state machinee---
        switch( statusLMB ) {
            case 0:
                dirX = dirY = 0;
                break;
            case 1:
                if( --prepareDelay == 0 ) statusLMB = 2;
                dirX += moveX;
                dirY += moveY;
                break;
            case 2:
                if( Mathf.Abs( dirX ) + Mathf.Abs( dirY ) < 1 ) {
                    statusLMB = 0;
                }else if( Mathf.Abs( dirX ) > Mathf.Abs( dirY ) ) {
                    if( dirX < 0 ) {
                        Debug.Log( "prepare swing left" );
                        mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Attack_SwingLeftIdle));
                        statusLMB = 3;
                    }else {
                        Debug.Log( "prepare swing right" );
                        mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Attack_SwingRightIdle));
                        statusLMB = 4;
                    }
                }else {
                    if( dirY < 0 ) {
                        Debug.Log( "prepare stab" );
                        mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Attack_StabIdle));
                        statusLMB = 5;
                    }else {
                        statusLMB = 0;
                    }
                }
                break;
            case 3:
                if( moveX > attackResponseSpeed ) {
                    statusLMB = 0;
                    Debug.Log( "swing left" );
                    mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Attack_SwingLeft));
                }
                break;
            case 4:
                if( moveX < -attackResponseSpeed ) {
                    statusLMB = 0;
                    Debug.Log( "swing right" );
                    mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Attack_SwingRight));
                }
                break;
            case 5:
                if( moveY > attackResponseSpeed ) {
                    statusLMB = 0;
                    Debug.Log( "stab" );
                    mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Attack_Stab));
                }
                break;
        }

        //---block state machine---
        switch( statusRMB ) {
            case 0:
                break;
            case 1:
                statusRMB = 0;
                if( moveX < -blockResponseSpeed ) {
                    Debug.Log( "block left" );
                    mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Block_Left));
                }
                else if( moveX > blockResponseSpeed ) {
                    Debug.Log( "block right" );
                    mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Block_Right));
                }
                else if( moveY > blockResponseSpeed ) {
                    Debug.Log( "block middle" );
                    mouseInputDelegate(new MouseInputStruct(MouseMovementsInput.Block_Front));
                }
                else{
                    statusRMB = 1;
                }
                break;
        }
    }
}
