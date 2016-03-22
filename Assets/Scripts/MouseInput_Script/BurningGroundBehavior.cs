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
	            yield return new WaitForSeconds( 1 );

	            foreach( GameObject enemy in enemies ) {
                    Swordsmanship.SwordsmanCharacter c = enemy.GetComponent<Swordsmanship.SwordsmanCharacter>();
                    
                    if(c)
                    {
                        c.BeHit(attacker, 160);
                        Debug.Log( "flag" );
                    }
                    
	            }
	        }
	    }
	}
}