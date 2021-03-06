﻿using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
	public class CircleController : MonoBehaviour {

		private Vector3 m_TargetPos;
		private float angle;
		[SerializeField] private float rotate_speed = 3.0f;
		[SerializeField] private float circle_speed = 8.0f;
		private SelectCharacterController sCC;

		// Use this for initialization
		void Start () {
			// fix resolution of screnn
			Screen.SetResolution (1473,733,true);

			angle = 0;
			sCC = GameObject.Find ("SelectCamera").GetComponent<SelectCharacterController> ();
		}
		
		// Update is called once per frame
		void Update () {
			angle += rotate_speed;
			if (angle > 360)
				angle -= 360;
			
			transform.rotation = Quaternion.Euler (90, angle, 0);
			m_TargetPos = sCC.GetTargetPos ();

			UpdatePosition ();
		}

		void UpdatePosition()
		{
			Vector3 tarPos = new Vector3 (m_TargetPos.x,m_TargetPos.y+0.01f,m_TargetPos.z);
			Vector3 newPos = Vector3.Lerp (transform.position, tarPos, Time.deltaTime * circle_speed);
			//transform.position = new Vector3 (newPos.x, transform.position.y, newPos.z);
			transform.position = new Vector3 (newPos.x, newPos.y, newPos.z);
		}
	}
}