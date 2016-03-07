using UnityEngine;
using System.Collections;

using Swordsmanship;

public class InitiateCharacter : MonoBehaviour {

	// MUST USE AWAKE!!!! FIRST INITIALIZE THIS ONE
	void Awake () {
		Debug.Log (ApplicationGlobals.selectedCharacterName);
		GameObject obj = GameObject.Instantiate (Resources.Load (ApplicationGlobals.selectedCharacterName)) as GameObject;
		obj.tag = "Player";
		obj.transform.position = new Vector3 (-1.5f, 0, 2.5f);
		obj.transform.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	//void Update () {
	//
	//}
}
