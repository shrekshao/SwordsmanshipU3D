using UnityEngine;
using System;
using System.Collections;


namespace Swordsmanship
{
    public class SpecialMoveSkill
    {
        protected String magicCastName;
        //tmp
        protected int num_stages;
        protected SoundSystem soundSystem;

        public SpecialMoveSkill() {

            //---cache reference to sound system---
            soundSystem = GameObject.FindGameObjectWithTag( "SoundSystem" ).GetComponent< SoundSystem >();
        }

        public int getStages() {
            return num_stages;
        }
        
        public virtual void CastSpecialMoveEffect(int stage_id, GameObject caster) { }
    }

    public class TornadoSword : SpecialMoveSkill{
        
        public TornadoSword() {
            num_stages = 7;
            magicCastName = "Storm";
        }

        // this should be a child class
        // Tornado sword
        public override void CastSpecialMoveEffect(int stage_id, GameObject caster)
        {
            if(stage_id == 2 || stage_id == 4 || stage_id == 6)
            {
				GameObject spell = GameObject.Instantiate(Resources.Load("Particles/" + magicCastName), caster.transform.position, caster.transform.rotation) as GameObject;
                spell.transform.position = caster.transform.position + caster.transform.forward * 0.1f;
                //spell.GetComponent<MagicCast>().InitMagicCast_Straight(caster, MagicCastType.ConstantMoving, caster.transform.forward);

                //spell.GetComponent<MagicCast>().InitMagicCast_Tracking(caster, GameObject.Find("AI-Di").transform);

				spell.GetComponent<MagicCast>().InitMagicCast(caster, MagicCastType.AccelerateMoving);

                //---play sound---
                soundSystem.play( ( int )SOUND_ID.SOUND_TORNADO, caster );
            }
        }
    }

    public class BurningGround : SpecialMoveSkill {

        public BurningGround() {
            num_stages = 4;
            magicCastName = "Burning Ground";
        }

        public override void CastSpecialMoveEffect(int stage_id, GameObject caster) {
            if( stage_id == 4 ) {

                //---spawn burning ground---
				GameObject spell = GameObject.Instantiate(
                    Resources.Load("Particles/" + magicCastName), 
                    caster.transform.position + new Vector3( 0, 0.1f, 0 ), 
                    caster.transform.rotation
                    ) as GameObject;
                
                //---deal damage to all enemies---
                spell.GetComponent< BurningGroundBehavior >().dealProlongedDamage( spell );

                //---play sound---
                soundSystem.play( ( int )SOUND_ID.SOUND_FIRE, caster );
            }
        }
    }

    public class Thunder : SpecialMoveSkill {
        
        public Thunder() {
            num_stages = 7;
            magicCastName = "Thunder";
        }

        public override void CastSpecialMoveEffect(int stage_id, GameObject caster) {
            if( true || stage_id == 0 ) {

                //---spawn thunder---
				GameObject spell = GameObject.Instantiate(
                    Resources.Load("Particles/" + magicCastName), 
                    caster.transform.position/* + new Vector3(UnityEngine.Random.Range( 3, 5 ), 0, UnityEngine.Random.Range( 3, 5 ) )*/,  
                    caster.transform.rotation
                    ) as GameObject;
                spell.transform.Rotate( new Vector3( -90, 0, 0 ) );

                //---deal damage to surrouding enemies---
                spell.GetComponent< ThunderBehavior >().dealDamage( spell );

                //---play sound---
                soundSystem.play( ( int )SOUND_ID.SOUND_THUNDER, caster );
            }
        }
    }
}
