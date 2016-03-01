using System;
using UnityEngine;

//namespace UnityStandardAssets.Characters.ThirdPerson
namespace Swordsmanship
{
    //[RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (SwordsmanCharacter))]
    public class AISwordsmanControl : MonoBehaviour
    {
        //public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public SwordsmanCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
	
		private Transform m_Cam;                  // A reference to the main camera in the scenes transform
		private Vector3 m_CamForward;             // The current forward direction of the camera
		private Vector3 m_Move;

		private bool prepare;
		private float attackCD;

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
			attackCD = 0.0f;

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
				attackCD = 0;
        }

		private void FixedUpdate()
		{
			Vector3 error = target.transform.position - this.transform.position;
			error.y = 0;

			//float h = error.sqrMagnitude / 4.0f;
			//float v = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis("Vertical");
			//float v = (Vector3.Angle(transform.forward,error) - 90) / 180 * Mathf.PI;
			//m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;

			m_Move = error / 3;

			character.Move(m_Move, false, false);

			//set attacking status
			if (attackCD <=0 && error.sqrMagnitude < 0.5f) 
			{
				if (!prepare) 
				{
					float r = UnityEngine.Random.value;
					if (0 <= r && r < 0.5) 
					{
					}
					else if (0.5 <= r && r < 1) 
					{
						character.AttackSwingLeftIdle (); 
						prepare = true;
						attackCD = 1.0f;
					}
				} 
				else 
				{
					character.AttackSwingLeftAttack ();
					prepare = false;
					attackCD = 1.5f;
				}
			}
		}

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
