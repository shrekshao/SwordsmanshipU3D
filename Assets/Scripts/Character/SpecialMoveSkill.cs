using UnityEngine;
using System;
using System.Collections;


namespace Swordsmanship
{
    public class SpecialMoveSkill
    {
        String magicCastName = "Storm";
        //tmp
        // this should be a child class
        public void CastSpecialMoveEffect(int stage_id, GameObject caster)
        {
            if(stage_id == 2 || stage_id == 4 || stage_id == 6)
            {
                GameObject spell = GameObject.Instantiate(Resources.Load("Particles/" + magicCastName), caster.transform.position, Quaternion.identity) as GameObject;
                spell.transform.position = caster.transform.position;
                spell.GetComponent<MagicCast>().InitMagicCast_Straight(caster, MagicCastType.ConstantMoving, caster.transform.forward);
            }
        }
    }

}
