using System;
using System.Collections;
using UnityEngine;

//namespace UnityStandardAssets.Characters.ThirdPerson
namespace Swordsmanship
{
    //[RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof(SwordsmanCharacter))]
    public class AISwordsmanControl : MonoBehaviour
    {
        //public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public SwordsmanCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;

        // Low level Attack status related
        private bool prepare;
        private float attackCD { get; set;}

        int attackDirection;

        int blockDirection;

        bool blocking;


        // Move static related
        private Vector3 dis_error;

		private bool pause;


        //tmp
        bool approach = true;

        //AI state machine
        AIState aiState;
        GameObject playerGO { get; set; }
        SwordsmanCharacter playerSwordsman { get; set; }


        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            //agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<SwordsmanCharacter>();
			target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

	        //agent.updateRotation = false;
	        //agent.updatePosition = true;

			m_Cam = Camera.main.transform;

            prepare = false;
            blocking = false;
			attackCD = 0.0f;

            dis_error = Vector3.zero;

            attackDirection = 0;
            blockDirection = 0;

            //tmp
            //StartCoroutine(RedecideStrategy());

			//tmp 
			pause = false;


            //ai init
            aiState = new AIStateNormal();
            playerGO = GameObject.FindGameObjectWithTag("Player");
            playerSwordsman = playerGO.GetComponent<SwordsmanCharacter>();
        }


        private void Update()
        {
            //if (target != null)
            //    agent.SetDestination(target.position);

            //if (agent.remainingDistance > agent.stoppingDistance)
            //    character.Move(agent.desiredVelocity, false, false);
            //else
            //    character.Move(Vector3.zero, false, false);


            //tmp for test

            //float r = UnityEngine.Random.value;

            //if(r < 0.15f)
            //{
            //    character.BlockLeft();
            //}
            //else if (r < 0.3f)
            //{
            //    character.BlockFront();
            //}
            //else if (r < 0.45f)
            //{
            //    character.BlockRight();
            //}
            //else
            //{
            //    character.AttackStabIdle();
            //    character.AttackStabAttack();
            //}

            //character.BlockRight();
            


            attackCD -= Time.deltaTime;
            if (attackCD < 0)
            {
                attackCD = 0;
            }

            ////tmp
            //if (Input.GetKeyDown (KeyCode.P))
            //	pause = !pause;

            aiState = aiState.Update(this);
        }

		//private void FixedUpdate()
		//{

  //          NormalMoveStrategy();

  //          NormalAttackStrategy();
		//}

        public void SetTarget(Transform target)
        {
            this.target = target;
        }



        // -1 is random
        public void NormalBlock(int dir = -1)
        {
            if(dir == -1)
            {
                dir = UnityEngine.Random.Range(0, 3);
            }
            
            switch(dir)
            {
                case 0:
                    character.BlockLeft();
                    break;
                case 1:
                    character.BlockRight();
                    break;
                case 2:
                    character.BlockFront();
                    break;
            }
            
        }

        public void NormalBlockExit()
        {
            character.BlockExit();
        }



        // -1 is random direction
        public void NormalAttack(int dir = -1)
        {
            
			if (pause)
				return;
			
            //set attacking status
            //if (dis_error.sqrMagnitude < 0.5f)
            //{
            if (attackCD <= 0)
            {
                if (!prepare)
                {
                    if (dir == -1)
                    {
                        attackDirection = UnityEngine.Random.Range(0, 3);
                    }
                    else
                    {
                        attackDirection = dir;
                    }
                    
                    switch(attackDirection)
                    {
                        case 0:
                            character.AttackSwingLeftIdle();
                            break;
                        case 1:
                            character.AttackSwingRightIdle();
                            break;
                        case 2:
                            character.AttackStabIdle();
                            break;
                    }
                    
                    prepare = true;
                    attackCD = 1.0f;
                }
                else
                {
                    switch (attackDirection)
                    {
                        case 0:
                            character.AttackSwingLeftAttack();
                            break;
                        case 1:
                            character.AttackSwingRightAttack();
                            break;
                        case 2:
                            character.AttackStabAttack();
                            break;
                    }
                    prepare = false;
                    attackCD = 1.5f;
                }
            }
            //}
        }

   //     public void NormalMove()
   //     {
			//if (pause)
			//{
			//	character.Move(Vector3.zero, false, false);
			//	return;
			//}
			
   //         dis_error = target.transform.position - this.transform.position;
   //         dis_error.y = 0;

   //         //float h = error.sqrMagnitude / 4.0f;
   //         //float v = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Vertical");
   //         //float v = (Vector3.Angle(transform.forward,error) - 90) / 180 * Mathf.PI;
   //         //m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

   //         m_Move = dis_error / 3;


   //         if(approach)
   //         {
   //             character.Move(m_Move, false, false);
   //         }
   //         else
   //         {
   //             character.Move(Vector3.zero, false, false);
   //         }


   //     }

        public void NormalMove(Vector3 move)
        {
            character.Move(move, false, false);
        }

        
        public void SpecialMoveCast(int specialMoveIndex, float stage_gap_seconds)
        {
            if (pause)
            {
                return;
            }

            character.SetSpecialMoveIndex(specialMoveIndex);

            int numStages = character.GetSkillStagesAt(specialMoveIndex);
            StartCoroutine(NextStageSpecialMove(numStages, stage_gap_seconds));
            

        }

        IEnumerator NextStageSpecialMove(int num_stages, float stage_gap_seconds)
        {
            for (int cur_stage = 0; cur_stage < num_stages; cur_stage++)
            {
                Debug.Log(cur_stage);
                character.NextStageTrigger(cur_stage);
                yield return new WaitForSeconds(stage_gap_seconds);
            }
            character.ExitSpecialMoveTrigger();
        }

        ////tmp
        //IEnumerator RedecideStrategy()
        //{
        //    while(true)
        //    {
        //        yield return new WaitForSeconds(5);

        //        approach = UnityEngine.Random.value > 0.5;
        //    }
            
        //}
    }
}
