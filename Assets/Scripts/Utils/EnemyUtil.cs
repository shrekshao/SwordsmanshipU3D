using UnityEngine;
using System.Collections;

public class EnemyUtil : MonoBehaviour {

	[SerializeField] private const float LockDistance = 5.0f;
	private GameObject[] enemies;

	// Use this for initialization
	void Start () {
		StoreEnemies ();
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

	public GameObject FindLockEnemy(Transform currentTransform,float distance = LockDistance)
	{
		//if (freeMove)
		//	return null;

		int index = -1;
		float nearestDis = float.MaxValue;
		for (int i = 0; i < enemies.Length; i++) {

			float dis = Vector3.Distance (currentTransform.position, enemies [i].transform.position);
			if (dis < nearestDis && dis < distance) 
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
