using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public class AIStateNormal : AIState
    {
        bool castingSpells = false;


        public override AIState Update(AISwordsmanControl ai)
        {
            if(!ai.character.BattleReady)
            {
                return this;
            }
            //ai.NormalMoveStrategy();
            
            //ai.NormalAttackStrategy();
            

            if(!castingSpells)
            {
                ai.SpecialMoveCast(1, 1.0f);
                castingSpells = true;
            }
            
            return this;
        }
    }
}

