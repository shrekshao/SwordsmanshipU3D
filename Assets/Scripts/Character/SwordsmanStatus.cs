using UnityEngine;
using System;
using System.Collections;

namespace Swordsmanship
{
    [Serializable]
    public class SwordsmanStatus
    {

        int hp;
        int mp;
        int strength;
        int agility;
        
        public SwordsmanStatus(){
            setHP( 100 );
            //setHP(10);
        }
        
        public bool loseHP( int lostHP ) {
            setHP( getHP() - lostHP );
            return hp <= 0;
        }
        
        public int getHP() {
            return hp;
        }

        public void setHP( int _hp ) {
            if( _hp < 0 ) _hp = 0;
            hp = _hp;
        }

    }
}

