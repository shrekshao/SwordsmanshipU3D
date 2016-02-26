using UnityEngine;
using System.Collections;

public class CharacterAttribute : MonoBehaviour {

    public float hp;
    private GUIBarScript hpBar;

    private float currentHP;
    
	void Start () {
	    currentHP = hp;
        hpBar = gameObject.GetComponentInChildren< GUIBarScript >();
	}
	
	void Update () {
	    if( currentHP - hp > 0.01F ) currentHP -= 0.008F;
        else if( currentHP - hp < -0.01F ) currentHP += 0.008F;
        if( hpBar != null ) hpBar.Value = currentHP;
	}
}
