using UnityEngine;
using System.Collections;

public class ThunderBehavior : MonoBehaviour {
    
	public void dealDamage( GameObject attacker ) {
	    
        GameObject[] enemies = EnemyUtil.GetEnemies();

        foreach( GameObject enemy in enemies ) {

            float distance = Vector3.Distance( attacker.transform.position, enemy.transform.position );

            if( distance < 2.0f ) {
                enemy.GetComponent< Swordsmanship.SwordsmanCharacter >().BeHit( attacker, 160 );
            }
        }
	}
}
