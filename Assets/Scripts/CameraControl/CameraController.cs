using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
	public class CameraController : MonoBehaviour {

		public GameObject target;

		public float dampling = 1;
		public float rotateSpeed = 5;
		public float LockDistance;

		Vector3 offset;

		private GameObject[] enemies;
		private GameObject lockEnemy;
		private bool freeMove;
		private float timer;

		// Use this for initialization
		void Start () {
			offset = target.transform.position - transform.position;
			StoreEnemies ();
			lockEnemy = null;
			freeMove = true;
			timer = 0.0f;
		}

		void StoreEnemies()
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

		void Update()
		{
			//if (Input.GetKeyDown (KeyCode.E)) 
			//{
			//	freeMove = !freeMove;
			//	lockEnemy = FindLockEnemy ();
			//}
			if (Input.GetKey(KeyCode.Q)) 
			{
				lockEnemy = FindLockEnemy ();
				LockEnemyCameraUpdate ();
				timer = 0.3f;
			}
		}

		// Late Update
		void LateUpdate () {
			
//			if (freeMove == false && lockEnemy != null) 
//			{
//				// find nearest enemy inside range LockDistance		
//				LockEnemyCameraUpdate ();
//			} 
//			else // normal moving camera control
//			{
//				//MouseAimCameraUpdate ();
//				FollowCameraUpdate();
//			}


			//tiemr handler
			if (timer > 0) {
				timer -= Time.deltaTime;
				LockEnemyCameraUpdate ();
			}
		}

		void LockEnemyCameraUpdate()
		{
			if (lockEnemy == null)
				return;
			
			Vector3 error = lockEnemy.transform.position - target.transform.position;
			error.y = 0;

			float desiredAngle = Mathf.Atan2 (error.x, error.z) * 180.0f / Mathf.PI;
			//float angle = Mathf.Lerp (target.transform.rotation.eulerAngles.y, desiredAngle,Time.deltaTime * dampling);

			Quaternion rotation = Quaternion.Euler (0, desiredAngle, 0);

			target.transform.rotation = rotation;

			transform.position = target.transform.position - (rotation * offset);
			transform.LookAt(target.transform);
		}

		GameObject FindLockEnemy()
		{
			//if (freeMove)
			//	return null;
			
			int index = -1;
			float nearestDis = float.MaxValue;
			for (int i = 0; i < enemies.Length; i++) {
			
				float dis = Vector3.Distance (target.transform.position, enemies [i].transform.position);
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

		void FollowCameraUpdate()
		{
			float currentAngle = transform.eulerAngles.y;
			float desiredAngle = target.transform.eulerAngles.y;
			float angle = Mathf.LerpAngle (currentAngle,desiredAngle,dampling*Time.deltaTime);

			Quaternion rotation = Quaternion.Euler(0,angle,0);
			transform.position = target.transform.position - (rotation * offset);

			transform.LookAt (target.transform);
		}

		void MouseAimCameraUpdate()
		{
			float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
			target.transform.Rotate(0, horizontal, 0);

			float desiredAngle = target.transform.eulerAngles.y;
			//float angle = Mathf.LerpAngle (transform.eulerAngles.y, desiredAngle,dampling*Time.deltaTime);


			Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
			transform.position = target.transform.position - (rotation * offset);
			//target.transform.rotation = rotation;

			transform.LookAt(target.transform);
		}
	}
}
