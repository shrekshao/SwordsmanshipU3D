using UnityEngine;
//using UnityStandardAssets.Characters.ThirdPerson;

namespace Swordsmanship
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class SwordsmanCharacter: MonoBehaviour
	{
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		[SerializeField] float m_JumpPower = 6f;
		[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
		[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.3f;

		Rigidbody m_Rigidbody;
        public Animator m_Animator;
		bool m_IsGrounded;
		float m_OrigGroundCheckDistance;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		Vector3 m_GroundNormal;
		float m_CapsuleHeight;
		Vector3 m_CapsuleCenter;
		CapsuleCollider m_Capsule;
		bool m_Crouching;


        //sword
        GameObject sword;
        Sword m_Sword;
        [SerializeField]
        Transform sword_hand_position;
        [SerializeField]
        Transform sword_back_position;


		//special move index
		public int m_specialMoveIndex;  //current operating special move index

        SpecialMoveSkill[] skills;


        //
        public bool BattleReady;

        //---swordsman status---
        SwordsmanStatus swordsmanStatus { get; set; }

        //---HP bar---
        GUIBarScript hpBar;

        void Start()
		{
			m_Animator = GetComponent<Animator>();
			m_Rigidbody = GetComponent<Rigidbody>();
			m_Capsule = GetComponent<CapsuleCollider>();
			m_CapsuleHeight = m_Capsule.height;
			m_CapsuleCenter = m_Capsule.center;

			m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			m_OrigGroundCheckDistance = m_GroundCheckDistance;


            //TMP, dynamic initialize
            InitSwordOnBack("Sword0");



			// initialize 
			m_specialMoveIndex = -1;
            InitSpecialMoveSkills();

            //---character status---
            swordsmanStatus = new SwordsmanStatus();

            //---HP bar---
            hpBar = GetComponentInChildren< GUIBarScript >();


            BattleReady = false;

        }

        void InitSwordOnBack(string sword_name)
        {
            sword = GameObject.Instantiate(Resources.Load(sword_name)) as GameObject;
            sword.transform.SetParent(sword_back_position);
            sword.transform.localPosition = Vector3.zero;
            sword.transform.localRotation = Quaternion.identity;

            sword.GetComponent<Collider>().enabled = false;

            m_Sword = sword.GetComponent<Sword>();
            m_Sword.master = this.gameObject;
            m_Sword.swordsman = this;
        }


        void InitSpecialMoveSkills()
        {
            //tmp
            skills = new SpecialMoveSkill[3];
            skills[1] = new SpecialMoveSkill();
        }


		public void Move(Vector3 move, bool crouch, bool jump)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;

			ApplyExtraTurnRotation();

			// control and velocity handling is different when grounded and airborne:
			if (m_IsGrounded)
			{
				HandleGroundedMovement(crouch, jump);
			}
			else
			{
				HandleAirborneMovement();
			}

			//ScaleCapsuleForCrouching(crouch);
			//PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}


		//void ScaleCapsuleForCrouching(bool crouch)
		//{
		//	if (m_IsGrounded && crouch)
		//	{
		//		if (m_Crouching) return;
		//		m_Capsule.height = m_Capsule.height / 2f;
		//		m_Capsule.center = m_Capsule.center / 2f;
		//		m_Crouching = true;
		//	}
		//	else
		//	{
		//		Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
		//		float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
		//		if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
		//		{
		//			m_Crouching = true;
		//			return;
		//		}
		//		m_Capsule.height = m_CapsuleHeight;
		//		m_Capsule.center = m_CapsuleCenter;
		//		m_Crouching = false;
		//	}
		//}

		//void PreventStandingInLowHeadroom()
		//{
		//	// prevent standing up in crouch-only zones
		//	if (!m_Crouching)
		//	{
		//		Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
		//		float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
		//		if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
		//		{
		//			m_Crouching = true;
		//		}
		//	}
		//}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);
			if (!m_IsGrounded)
			{
				m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
			}
            else
            {
                m_Animator.SetFloat("Jump", 0.0f);
            }

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
			float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
			if (m_IsGrounded)
			{
				m_Animator.SetFloat("JumpLeg", jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
			m_Rigidbody.AddForce(extraGravityForce);

			m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool crouch, bool jump)
		{
            // check whether conditions are right to allow a jump:

            //if (jump && !crouch && (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")
            //    || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("GroundedDefault")) )

            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("GroundedDefault"))
            {
				// jump!
				m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
				m_IsGrounded = false;
				m_Animator.applyRootMotion = false;
				m_GroundCheckDistance = 0.1f;
			}
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (m_IsGrounded && Time.deltaTime > 0)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = m_Rigidbody.velocity.y;
				m_Rigidbody.velocity = v;
			}
		}


		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance),Color.blue);
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				m_Animator.applyRootMotion = false;
			}
		}


        // --------- Animator Control related ----------------

        //Attack
        public void AttackSwingLeftIdle()
        {
            m_Animator.SetBool("SwingLeftStart",true);
            m_Animator.SetBool("SwingLeftAttack", false);
        }

        public void AttackSwingLeftAttack()
        {
            m_Animator.SetBool("SwingLeftAttack",true);
            m_Animator.SetBool("SwingLeftStart", false);
        }

        public void AttackSwingRightIdle()
        {
            m_Animator.SetBool("SwingRightStart", true);
            m_Animator.SetBool("SwingRightAttack", false);
        }

        public void AttackSwingRightAttack()
        {
            m_Animator.SetBool("SwingRightAttack", true);
            m_Animator.SetBool("SwingRightStart", false);
        }

        public void AttackStabIdle()
        {
            m_Animator.SetBool("StabStart", true);
            m_Animator.SetBool("StabAttack", false);
        }

        public void AttackStabAttack()
        {
            m_Animator.SetBool("StabAttack", true);
            m_Animator.SetBool("StabStart", false);
        }

        //Block
        public void BlockFront()
        {
            m_Animator.SetBool("BlockFront", true);
        }
        public void BlockLeft()
        {
            m_Animator.SetBool("BlockLeft", true);
        }
        public void BlockRight()
        {
            m_Animator.SetBool("BlockRight", true);
        }

        public void BlockExit()
        {
            m_Animator.SetBool("BlockFront", false);
            m_Animator.SetBool("BlockLeft", false);
            m_Animator.SetBool("BlockRight", false);
        }
			
        //Idle, clear all bool
        public void IdleClearStates()
        {
            m_Animator.SetBool("BlockFront", false);
            m_Animator.SetBool("BlockLeft", false);
            m_Animator.SetBool("BlockRight", false);

            m_Animator.SetBool("SwingRightStart", false);
            m_Animator.SetBool("SwingRightAttack", false);

            m_Animator.SetBool("SwingLeftStart", false);
            m_Animator.SetBool("SwingLeftAttack", false);

            m_Animator.SetBool("StabStart", false);
            m_Animator.SetBool("StabAttack", false);
        }


        //Draw sword, change sword parent
        public void DrawSwordChangeParent()
        {
            sword.transform.SetParent(sword_hand_position);
            sword.transform.localPosition = Vector3.zero;
            sword.transform.localRotation = Quaternion.identity;

            m_Animator.SetBool("UpperLocked", false);
            BattleReady = true;
        }

		// Special moves 
		public void NextStageTrigger(int special_move_stage)
		{
			m_Animator.SetTrigger ("NextStage");

            if(m_specialMoveIndex >= 0)
            {
                skills[m_specialMoveIndex].CastSpecialMoveEffect(special_move_stage, gameObject);
            }
           
		}
		public void ExitSpecialMoveTrigger()
		{
            m_specialMoveIndex = -1;
            m_Animator.ResetTrigger("NextStage");
            m_Animator.SetTrigger ("ExitSpecialMove");
            
        }
		public void StopSpecialMoveTrigger()
		{
            m_specialMoveIndex = -1;
            m_Animator.ResetTrigger("NextStage");
            m_Animator.SetTrigger ("StopSpecialMove");
			
		}
		public void SetSpecialMoveIndex(int speicalMoveIndex)
		{
            //m_Animator.SetInteger("SpecialMoveID", m_specialMoveIndex);
            m_specialMoveIndex = speicalMoveIndex;
            m_Animator.SetInteger("SpecialMoveID", speicalMoveIndex);

            m_Animator.ResetTrigger("NextStage");
            m_Animator.ResetTrigger("StopSpecialMove");
            m_Animator.ResetTrigger("ExitSpecialMove");
            
        }
        // -----------------------------------------------------------


        // --------- Enviornment Interacting------------------
        public void EnableSwordAttack()
        {
            sword.GetComponent<Collider>().enabled = true;
            m_Sword.sword_state = SwordState.Attack;
        }

        public void DisableSwordAttack()
        {
            sword.GetComponent<Collider>().enabled = false;
        }

        public void EnableSwordBlock()
        {
            sword.GetComponent<Collider>().enabled = true;
            m_Sword.sword_state = SwordState.Block;
        }

        public void DisableSwordBlock()
        {
            sword.GetComponent<Collider>().enabled = false;
        }



        // --------- Attack Effect-----------------
        public void AttackCharacter(Collider other)
        {
            //attack hit!
            if(other.tag == "Attackable")
            {
                Vector3 dir = other.gameObject.transform.position - transform.position;
                other.GetComponent<Rigidbody>().AddForce(100 * dir.normalized);
            }
			else if(other.tag == "Human" || other.tag == "Player")
            {
                SwordsmanCharacter target = other.gameObject.GetComponent<SwordsmanCharacter>();

                // Knock back
                target.BeHit(gameObject);
                //
            }
            

        }


        public void BeHit(GameObject attacker_gb, float knockbackForce = 160)
        {
            Debug.Log("Be Hit!!!");
            m_Animator.SetTrigger("Damaged");

            //---lose HP and update HP bar---
            swordsmanStatus.loseHP( Random.Range( 10, 20 ) );
            if( hpBar != null ) hpBar.Value = swordsmanStatus.getHP() / 100.0f;

            //be Knocked back
            Vector3 dir = transform.position - attacker_gb.transform.position;

            GetComponent<Rigidbody>().AddForce(knockbackForce * dir.normalized);
        }

        
        public void BeBlocked()
        {
            Debug.Log("Be Blocked!!!!");
			m_Animator.SetBool ("isBlocked",true);
        }
		


        //skill special move uitl
        public int GetSkillStagesAt(int id)
        {
            return skills[id].getStages();
        }
        

        
    }
}
