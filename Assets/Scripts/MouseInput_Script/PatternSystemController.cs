using UnityEngine;
using System.Collections;

public class PatternSystemController : MonoBehaviour {
    
    public int nPrompts;
    public GameObject prompt;
    public float minDistance;
    public float maxDistance;
    public bool[] isTouched;

    public GameObject[] prompts;

	void Start () {
        prompts = new GameObject[ nPrompts ];
        isTouched = new bool[ nPrompts ];
	}
	
    void NewPattern() {
        
        //---clear last prompts---
        ClearPattern();

        //---generate prompts---
        for( int i = 0; i < nPrompts; ++i ) {

            Vector2 rand1 = Random.insideUnitCircle;
            float rand2 = Random.Range( minDistance, maxDistance );

            //---initialize prompt---
            if( prompts[ i ] ) Destroy( prompts[ i ] );
            prompts[ i ] = Instantiate( prompt, new Vector3(), new Quaternion() ) as GameObject;
            prompts[ i ].transform.parent = gameObject.transform;
            prompts[ i ].transform.localPosition = new Vector3( rand1.x, rand1.y, 0 ) * rand2;
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


	void Update () {

        //---generate patter---
        if( Input.GetKeyDown( KeyCode.Space ) ) {
            NewPattern();
        }
        
        //---check touch order---
        for( int i = 1; i < nPrompts; ++i ) {
            if( isTouched[ i ] && !isTouched[ i - 1 ] ) {
                ClearPattern();
            }
        }
	}
}
