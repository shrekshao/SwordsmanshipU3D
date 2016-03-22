using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
	public class BurningGroundBehavior : MonoBehaviour {
	    
	    private GameObject attacker;

		public void dealProlongedDamage( GameObject _attacker ) {
	        
	        attacker = _attacker;
	        StartCoroutine( damage() );
	    }

	    IEnumerator damage() {

	        GameObject[] enemies = EnemyUtil.GetEnemies();

	        for( int i = 0; i < 3; ++i ) {
	            foreach( GameObject enemy in enemies ) {
	                yield return new WaitForSeconds( 1 );
	                enemy.GetComponent< Swordsmanship.SwordsmanCharacter >().BeHit( attacker, 160 );
	            }
	        }
	    }
	}
}