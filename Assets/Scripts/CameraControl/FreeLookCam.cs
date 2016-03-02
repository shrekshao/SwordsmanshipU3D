using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Swordsmanship
{
    public class FreeLookCam : PivotBasedCameraRig
    {
        // This script is designed to be placed on the root object of a camera rig,
        // comprising 3 gameobjects, each parented to the next:

        // 	Camera Rig
        // 		Pivot
        // 			Camera

        [SerializeField] private float m_MoveSpeed = 1f;                      // How fast the rig will move to keep up with the target's position.
        [Range(0f, 10f)] [SerializeField] private float m_TurnSpeed = 1.5f;   // How fast the rig will rotate from user input.
        [SerializeField] private float m_TurnSmoothing = 0.1f;                // How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness
        [SerializeField] private float m_TiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
        [SerializeField] private float m_TiltMin = 45f;                       // The minimum value of the x axis rotation of the pivot.
        [SerializeField] private bool m_LockCursor = false;                   // Whether the cursor should be hidden and locked.
        [SerializeField] private bool m_VerticalAutoReturn = false;           // set wether or not the vertical axis should auto return
		[SerializeField] private GameObject playerObj;
		[SerializeField] private float LockDistance = 5.0f;

        private float m_LookAngle;                    // The rig's y axis rotation.
        private float m_TiltAngle;                    // The pivot's x axis rotation.
        private const float k_LookDistance = 100f;    // How far in front of the pivot the character's look target is.
		private Vector3 m_PivotEulers;
		private Quaternion m_PivotTargetRot;
		private Quaternion m_TransformTargetRot;
		private SwordsmanCharacter swordmanCharacter;
		private GameObject[] enemies;

        protected override void Awake()
        {
            base.Awake();
            // Lock or unlock the cursor.
            Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_LockCursor;
			m_PivotEulers = m_Pivot.rotation.eulerAngles;

	        m_PivotTargetRot = m_Pivot.transform.localRotation;
			m_TransformTargetRot = transform.localRotation;

			swordmanCharacter = playerObj.GetComponent<SwordsmanCharacter> ();
			StoreEnemies ();
        }


        protected void Update()
        {
			if (!Input.GetKey ("mouse 0") && !Input.GetKey ("mouse 1")
			   && swordmanCharacter.m_specialMoveIndex == -1) 
			{
				HandleRotationMovement ();
			}

			if (m_LockCursor && Input.GetMouseButtonUp(0))
            {
                Cursor.lockState = m_LockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !m_LockCursor;
            }
			if (Input.GetKey (KeyCode.Q) || Input.GetKey("mouse 2")) 
			{
				LockEnemyRotation ();
			}
        }

		void LockEnemyRotation()
		{

			GameObject obj = FindLockEnemy ();
			if (obj == null)
				return;

			Vector3 error = obj.transform.position - transform.position;
			error.y = 0;

			float desiredAngle = Mathf.Atan2 (error.x, error.z) * 180.0f / Mathf.PI;
			m_LookAngle = desiredAngle;
			m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);
			transform.localRotation = m_TransformTargetRot;

		}

        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        protected override void FollowTarget(float deltaTime)
        {
            if (m_Target == null) return;
            // Move the rig towards target position.
            transform.position = Vector3.Lerp(transform.position, m_Target.position, deltaTime*m_MoveSpeed);
        }


        private void HandleRotationMovement()
        {
			if(Time.timeScale < float.Epsilon)
			return;

            // Read the user input
            var x = CrossPlatformInputManager.GetAxis("Mouse X");
            var y = CrossPlatformInputManager.GetAxis("Mouse Y");

            // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
            m_LookAngle += x*m_TurnSpeed;

            // Rotate the rig (the root object) around Y axis only:
            m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

            if (m_VerticalAutoReturn)
            {
                // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
                // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
                // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
                m_TiltAngle = y > 0 ? Mathf.Lerp(0, -m_TiltMin, y) : Mathf.Lerp(0, m_TiltMax, -y);
            }
            else
            {
                // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
                m_TiltAngle -= y*m_TurnSpeed;
                // and make sure the new value is within the tilt range
                m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
            }

            // Tilt input around X is applied to the pivot (the child of this object)
			m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y , m_PivotEulers.z);

			if (m_TurnSmoothing > 0)
			{
				m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
			}
			else
			{
				m_Pivot.localRotation = m_PivotTargetRot;
				transform.localRotation = m_TransformTargetRot;
			}
        }

		private void StoreEnemies()
		{
			GameObject[] gos = GameObject.FindObjectsOfType (typeof(GameObject)) as GameObject[];
			var enemyList = new System.Collections.Generic.List<GameObject>();

			foreach (GameObject ene in gos) 
			{

				if (ene.layer == LayerMask.NameToLayer ("EnemyLayer")) 
				{
					enemyList.Add (ene);
				}
			}
			enemies = enemyList.ToArray ();
		}

		public GameObject FindLockEnemy()
		{
			//if (freeMove)
			//	return null;

			int index = -1;
			float nearestDis = float.MaxValue;
			for (int i = 0; i < enemies.Length; i++) {

				float dis = Vector3.Distance (playerObj.transform.position, enemies [i].transform.position);
				if (dis < nearestDis && dis < LockDistance) 
				{
					nearestDis = dis;
					index = i;
				}
			}

			if (index != -1)
				return enemies [index];
			else 
				return null;

		}

    }
}
