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
        }
        
        public void loseHP( int lostHP ) {
            setHP( getHP() - lostHP );
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

