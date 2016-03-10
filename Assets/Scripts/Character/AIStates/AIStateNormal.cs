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
        public const int MAX_IDLE_TIME = 100;
        public const float IDLE_RATE = 0.1f;
        public const float SPECIAL_RATE = 0.5f;
        

        
        float attackRadius = 1.0f;
        float blockRadius = 2.0f;

        int idleTime = 0;

        public override AIState Update(AISwordsmanControl ai)
        {
            if(!ai.character.BattleReady)
            {
                return this;
            }

            if(ai.castingSpells)
            {
                return this;
            }
            //ai.NormalMove();


            // idle ING...
            if(idleTime > 0)
            {
                idleTime--;
                return this;
            }


            // go to idle?
            float r = Random.value;
            if(r < IDLE_RATE)
            {
                idleTime = Random.Range(0, MAX_IDLE_TIME);
                return this;
            }



            // calculate distance
            Vector3 disVec3 = ai.target.transform.position - ai.character.transform.position;
            float dis = disVec3.magnitude;
            disVec3.Normalize();

            Debug.DrawLine(ai.character.transform.position + Vector3.up, ai.character.transform.position + disVec3 + Vector3.up);


            //int upperBodyState = ai.character.m_Animator.GetCurrentAnimatorStateInfo(1).GetHashCode();
            int targetUpperBodyState = ai.targetCharacter.m_Animator.GetCurrentAnimatorStateInfo(1).fullPathHash;
            //// block?
            if (dis < blockRadius)
            {
                ai.character.Move(0.1f * disVec3, false, false);
                if ( SwordsmanCharacter.hash_attackLeftIdle == targetUpperBodyState)
                {
                    //Debug.Log("block left");
                    ai.NormalBlock(1);
                    return this;
                }
                else if (SwordsmanCharacter.hash_attackRightIdle == targetUpperBodyState)
                {
                    ai.NormalBlock(0);
                    return this;
                }
                else if (SwordsmanCharacter.hash_attackStabIdle == targetUpperBodyState)
                {
                    ai.NormalBlock(2);
                    return this;
                }
                else
                {
                    ai.NormalBlockExit();
                }
                
            }
            else
            {
                ai.NormalBlockExit();
            }


            // attack?
            if (dis < attackRadius)
            {
                return new AIStateInstantAttack();
            }
            else
            {
                ai.character.Move(0.2f * disVec3, false, false);
            }


            //// cast special
            //if(Random.value < SPECIAL_RATE)
            //{
            //    return new AIStateInstantCastSpeical();
            //}


            return this;
        }
    }
}

