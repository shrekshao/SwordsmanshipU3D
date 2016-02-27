using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets.Characters.ThirdPerson;

namespace Swordsmanship
{
    [RequireComponent(typeof (SwordsmanCharacter))]
    public class SwordsmanControl : MonoBehaviour
    {
        private SwordsmanCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


        //private MouseInputStruct mouseInput;

		// special moves varible
		//int m_specialMoveIndex;
		int m_specialMoveStage;
		float[] m_specialMoveCDTime={0,0,0};

        private PatternSystemController psController;
        private SpecialSystem specialSystem;

        private void Start()
        {
            //---get pattern system controller---
            psController = GameObject.FindGameObjectWithTag( "PatternSystemController" ).GetComponent< PatternSystemController >();

            //---get special system---
            specialSystem = GameObject.FindGameObjectWithTag( "SpecialSystem" ).GetComponent< SpecialSystem >();

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<SwordsmanCharacter>();


            // init mouseInput
            //mouseInput = new MouseInputStruct();

            MouseBehavior.mouseInputDelegate += MouseInputHandle;

			//special move initialization
			m_Character.m_specialMoveIndex = -1;
			m_specialMoveStage = 0;
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown("Jump");
                if(m_Jump)
                {
                    Debug.Log(m_Jump);
                }
                
            }

			// special move handler
			SpecialMoveHandle ();
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Horizontal");
            float v = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);


            // DELETE ME: Test input
            

            //if(Input.GetKey(KeyCode.B))
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Block_Front;
            //}
            //else
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Block_Exit;
            //}


            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Attack_SwingLeftIdle;
            //}

            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    mouseInput.mouseMovementInput = MouseMovementsInput.Attack_SwingLeft;
            //}

            /////////////////////////


            //attack
            //Attack();
            


            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;

        }

		private void SpecialMoveHandle()
		{	
			for (int i = 0; i < 3; i++) 
			{
				m_specialMoveCDTime[i] -= Time.deltaTime;
				if (m_specialMoveCDTime[i] < 0) {
					m_specialMoveCDTime[i] = 0;
				}

				if (m_specialMoveCDTime[i] > 0) { 	//not cooled down, not receive input 

					float percent = 1.0f - m_specialMoveCDTime[i] / SpecialMovesDefination.cdInterval [i];
					// percentCoolDown
					//Debug.Log ("percentCoolDown: index = " + m_Character.m_specialMoveIndex + ", percent = " + percent);
					specialSystem.updateCooldown (i, percent);
					//return;
				}
			}

			// special move playing
			if (m_Character.m_specialMoveIndex >= 0) 
				return;
			
			if (Input.GetKey (KeyCode.Alpha1) && m_specialMoveCDTime[0] == 0) 
			{
				m_Character.m_specialMoveIndex = 0;
				m_Character.SetSpecialMoveIndex ();
			} 
			else if (Input.GetKey (KeyCode.Alpha2) && m_specialMoveCDTime[1] == 0) 
			{
				m_Character.m_specialMoveIndex = 1;
				m_Character.SetSpecialMoveIndex ();
			} 
			else if (Input.GetKey (KeyCode.Alpha3) && m_specialMoveCDTime[2] == 0) 
			{
				m_Character.m_specialMoveIndex = 2;
				m_Character.SetSpecialMoveIndex ();
			}

			if (m_Character.m_specialMoveIndex >= 0) 
			{
				m_specialMoveCDTime[m_Character.m_specialMoveIndex] = SpecialMovesDefination.cdInterval [m_Character.m_specialMoveIndex];

				// perform special move pattern system
				//Debug.Log ("performSpecial:" + m_Character.m_specialMoveIndex);
                psController.performSpecial( m_Character.m_specialMoveIndex );
			}
		}

		public void SpecialMoveNextStage()
		{
			if (m_Character.m_specialMoveIndex < 0)
				return;
			
			m_specialMoveStage ++;

			if (m_specialMoveStage > SpecialMovesDefination.specialMoveSteps[m_Character.m_specialMoveIndex]) {
				m_specialMoveStage = 0; // finish movement
				//m_animator.SetTrigger("SpecialMoveExitTrigger");
				//m_Character.ExitSpecialMoveTrigger();

				m_Character.m_specialMoveIndex = -1;
				m_Character.SetSpecialMoveIndex ();
			} 
			else 
			{
				//m_animator.SetTrigger ("NextStepTrigger");
				m_Character.NextStageTrigger();
			}
		}

		public void SpecialMoveStop()
		{
			m_specialMoveStage = 0;

			m_Character.m_specialMoveIndex = -1;
			m_Character.SetSpecialMoveIndex ();
			//m_animator.SetTrigger ("Stop");

			m_Character.StopSpecialMoveTrigger();
		}

        private void MouseInputHandle(MouseInputStruct m_input)
        {

            switch(m_input.mouseMovementInput)
            {
                case MouseMovementsInput.Idle:
                    m_Character.IdleClearStates();
                    break;
                case MouseMovementsInput.Attack_SwingLeftIdle:
                    m_Character.AttackSwingLeftIdle();
                    break;
                case MouseMovementsInput.Attack_SwingLeft:
                    m_Character.AttackSwingLeftAttack();
                    break;
                case MouseMovementsInput.Attack_SwingRightIdle:
                    m_Character.AttackSwingRightIdle();
                    break;
                case MouseMovementsInput.Attack_SwingRight:
                    m_Character.AttackSwingRightAttack();
                    break;
                case MouseMovementsInput.Attack_StabIdle:
                    m_Character.AttackStabIdle();
                    break;
                case MouseMovementsInput.Attack_Stab:
                    m_Character.AttackStabAttack();
                    break;

                case MouseMovementsInput.Block_Front:
                    m_Character.BlockFront();
                    break;
                case MouseMovementsInput.Block_Left:
                    m_Character.BlockLeft();
                    break;
                case MouseMovementsInput.Block_Right:
                    m_Character.BlockRight();
                    break;

                case MouseMovementsInput.Block_Exit:
                    m_Character.BlockExit();
                    break;

                default:
                    break;
            }

        }


    }
}
