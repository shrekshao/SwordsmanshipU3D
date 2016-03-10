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


    public class AIStateInstantCastSpeical : AIState
    {
        //bool castingSpells = false;
        //float attackRadius = 10.0f;
        public const float SPECIAL_GAP = 1.0f;

        public override AIState Update(AISwordsmanControl ai)
        {
            Vector3 disVec3 = ai.target.position - ai.character.transform.position;
            ai.character.Move(disVec3.normalized * 0.5f, false, false);

            if (!ai.castingSpells)
            {
                ai.castingSpells = true;
                ai.SpecialMoveCast(1, SPECIAL_GAP);

                
            }

            return new AIStateNormal();
        }
    }
}

