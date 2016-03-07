using UnityEngine;
using System.Collections;

namespace Swordsmanship
{

    /**
    * this ai state outline:
    * - attack when enter a radius, swing immediately
    * - safety first: block when necessary
    * - keep a distance
    * - 
    */


    public class AIStateNormal : AIState
    {
        bool castingSpells = false;
        float attackRadius = 10.0f;

        public override AIState Update(AISwordsmanControl ai)
        {
            if(!ai.character.BattleReady)
            {
                return this;
            }
            //ai.NormalMove();
            
            //ai.NormalAttack();
            

            //if( !ai.character.m_Animator.GetBool("UpperLocked"))
            if( !castingSpells )
            {
                ai.SpecialMoveCast(1, 3.0f);

                castingSpells = true;
            }
            
            return this;
        }
    }
}

