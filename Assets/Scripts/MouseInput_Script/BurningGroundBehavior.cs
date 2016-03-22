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

                    Swordsmanship.SwordsmanCharacter c = enemy.GetComponent<Swordsmanship.SwordsmanCharacter>();
                    
                    if(c)
                    {
                        c.BeHit(attacker, 160);
                    }
                    
	            }
	        }
	    }
	}
}