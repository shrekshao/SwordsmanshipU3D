using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

namespace Swordsmanship
{
	public class SelectCharacterController : MonoBehaviour {

		[SerializeField] private float m_TurnSmoothing;
		[SerializeField] private float m_TiltMax = 75f;                   
		[SerializeField] private float m_TiltMin = 45f; 
		[SerializeField] private float m_TurnSpeed = 3.0f;
		private float m_LookAngle;
		private float m_TiltAngle;
		private Quaternion m_TransformTargetRot;
		private Quaternion m_PivotTargetRot;
		private Transform m_Cam;
		private Transform m_Pivot;
		private Vector3 m_PivotEulers;
		private float m_minDistance;
		private float m_ScrollSpeed;
		private Vector3 m_TargetPos;

		/// <summary>
		/// The players objs.
		/// </summary>
		private GameObject[] players; 
		private int selectedIndex;

		// GUI
		//[SerializeField] private GameObject textObj;
		[SerializeField] private Text text;

		// Use this for initialization
		protected virtual void Awake()
		{
			// find the camera in the object hierarchy
			m_Cam = GetComponentInChildren<Camera>().transform;
			m_Pivot = m_Cam.parent;
			m_PivotEulers = m_Pivot.rotation.eulerAngles;

			m_PivotTargetRot = m_Pivot.transform.localRotation;
			m_TransformTargetRot = transform.localRotation;

			m_minDistance = 0.6f;
			m_ScrollSpeed = 0.2f;

			StorePlayers ();

			//text = textObj.GetComponent<Text> ();
		}

	//	void Start () {
	//		StorePlayers ();
	//	}

		void StorePlayers()
		{
			players = GameObject.FindGameObjectsWithTag ("Human");
			if (players.Length > 0) 
			{
				selectedIndex = 0;
				transform.position = players [selectedIndex].transform.position;
			}
			else 
			{
				selectedIndex = -1;
			}
			players [selectedIndex].GetComponent<Animator> ().SetTrigger ("DrawSword");
			//m_TargetPos = players [selectedIndex].GetComponent<Transform> ().position;
		}

		// Update is called once per frame
		void Update () {
			HandleSelectObject ();
			HandleTargetPosition ();
			HandleRotationMovement ();
			HandleMouseScrollMovement ();
			UpdateGUI ();
		}

		void HandleTargetPosition()
		{
			m_TargetPos = players [selectedIndex].GetComponent<Transform> ().position;
		//	m_Pivot.position = Vector3.Lerp (m_Pivot.position, m_TargetPos, Time.deltaTime * m_TurnSpeed);
			transform.position = Vector3.Lerp(transform.position, m_TargetPos, Time.deltaTime * m_TurnSpeed);
		}

		private void UpdateGUI()
		{
			text.text = "    " + players [selectedIndex].name;
		}

		private void HandleSelectObject()
		{
			//Vector3 deltaPos = new Vector3 (0, 0, 0);
			if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.LeftArrow)) {
				int oldIdx = selectedIndex;
				selectedIndex = selectedIndex - 1;

				if (selectedIndex < 0)
					selectedIndex = players.Length - 1;

				players [selectedIndex].gameObject.GetComponent<Animator> ().SetTrigger ("DrawSword");
				players [oldIdx].gameObject.GetComponent<Animator> ().SetTrigger ("DeSelected");

			//	deltaPos = players [selectedIndex].transform.position - players [oldIdx].transform.position;
			}

			if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {
				int oldIdx = selectedIndex;
				selectedIndex = selectedIndex + 1;

				if (selectedIndex >= players.Length) 
					selectedIndex = 0;
				
				players [selectedIndex].gameObject.GetComponent<Animator> ().SetTrigger ("DrawSword");
				players [oldIdx].gameObject.GetComponent<Animator> ().SetTrigger ("DeSelected");

			//	deltaPos = players [selectedIndex].transform.position - players [oldIdx].transform.position;
			}

			//transform.position = transform.position + deltaPos;
		}

		private void HandleMouseScrollMovement()
		{
			float delta = CrossPlatformInputManager.GetAxisRaw ("Mouse ScrollWheel");
			Vector3 newPosition = m_Pivot.position;
			if(delta > 0)
				newPosition = m_Cam.transform.position + m_ScrollSpeed * m_Cam.transform.forward;
			if(delta < 0)
				newPosition = m_Cam.transform.position - m_ScrollSpeed * m_Cam.transform.forward;

			if (Vector3.Distance (newPosition, m_Pivot.position) > m_minDistance)
				m_Cam.transform.position = newPosition;
		}

		private void HandleRotationMovement()
		{
			if(Time.timeScale < float.Epsilon)
				return;

			// Read the user input
			float x=0;
			float y=0;

			if (CrossPlatformInputManager.GetButton ("Fire1")) 
			{
				x = CrossPlatformInputManager.GetAxis ("Mouse X");
				y = CrossPlatformInputManager.GetAxis ("Mouse Y");
			}

			// Adjust the look angle by an amount proportional to the turn speed and horizontal input.
			m_LookAngle += x*m_TurnSpeed;

			// Rotate the rig (the root object) around Y axis only:
			m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);

			// Tilting
			m_TiltAngle -= y*m_TurnSpeed;
			m_TiltAngle = Mathf.Clamp(m_TiltAngle, m_TiltMin, m_TiltMax);

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

		public void StartGame()
		{
			Debug.Log ("Start !!!");
			ApplicationGlobals.selectedCharacterName = players [selectedIndex].name;
			SceneManager.LoadScene ("Battle");
		}

		public Vector3 GetTargetPos()
		{
			return m_TargetPos;
		}
	}
}
