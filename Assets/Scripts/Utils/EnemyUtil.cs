using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
	public class EnemyUtil : MonoBehaviour {

		[SerializeField] private const float LockDistance = 5.0f;
		public static GameObject[] enemies;

		// Use this for initialization
		void Start () {
			StoreEnemies ();
		}

		public static void StoreEnemies()
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

		static public GameObject[] GetEnemies()
		{
			return enemies;
		}

		public static GameObject FindLockEnemy(Transform currentTransform,float distance = LockDistance)
		{
			//if (freeMove)
			//	return null;
			StoreEnemies();

			int index = -1;
			float nearestDis = float.MaxValue;
			for (int i = 0; i < enemies.Length; i++) {

				float dis = Vector3.Distance (currentTransform.position, enemies [i].transform.position);
				if (!enemies[i].GetComponent<SwordsmanCharacter>().isDead() && dis < nearestDis && dis < distance) 
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

		// Update is called once per frame
		void Update () {
		
		}
	}
}