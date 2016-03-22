using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Swordsmanship
{
	public class GameManager : MonoBehaviour {

		private float timeScale;

        //private GameObject[] players;
        System.Collections.Generic.List<GameObject> players;
        //ArrayList players;

        //public EnemyGenerator enemyGenerator;
        public Transform[] birthPoint;

        int MAX_RESPAWN_ENEMY_TIME = 800;
        int respawnEnemyTime;

		[SerializeField]
		Text nKillText;
		int nKills = 0;

		GameObject playerCharacter;
		SwordsmanCharacter playerCharacter_Character;

        // Use this for initialization
        void Start () {
			timeScale = Time.timeScale;
			StorePlayer ();
            //respawnEnemyTime = MAX_RESPAWN_ENEMY_TIME;
            respawnEnemyTime = 1;

			//register delegate
			SwordsmanCharacter.dieEvent += HandleDieEvent;
			UpdateText ();
        }

		void HandleDieEvent(GameObject obj)
		{
			players.Remove (obj);
			IncreaseNKills ();
			UpdateText ();
		}

		void IncreaseNKills()
		{
			nKills++;
			UpdateText ();
		}

		void UpdateText()
		{
			nKillText.text = nKills.ToString() + " KILLS";
			nKillText.GetComponent<TextFlashScript> ().init ();
		}
			
		void StorePlayer()
		{
			GameObject[] gos = GameObject.FindGameObjectsWithTag("Human") as GameObject[];
			GameObject[] gos2 = GameObject.FindGameObjectsWithTag ("Player") as GameObject[];

            playerCharacter = gos2[0];
            playerCharacter_Character = playerCharacter.GetComponent<SwordsmanCharacter>();

			var objList = new System.Collections.Generic.List<GameObject>();

			foreach (GameObject player in gos) 
			{
				objList.Add (player);
			}

			foreach (GameObject player in gos2) 
			{
				objList.Add (player);
			}


            //players = objList.ToArray ();
            //players = objList;
            players = objList;
		}

		void HandlePause()
		{
			if (Input.GetKeyDown (KeyCode.P)) 
			{
				//Time.timeScale = timeScale;
				if (Time.timeScale > 0) 
				{
					Time.timeScale = 0;
					//Cursor.lockState = CursorLockMode.None;

				} 
				else 
				{
					Time.timeScale = timeScale;
					//Cursor.lockState = CursorLockMode.Locked;
				}
			}
		}

		void GameFinish()
		{
			//for (int i = 0; i < players.Length; i++) {
			//	if (players [i].GetComponent<SwordsmanCharacter>().isDead ()) {
			//		//if (players [i].layer == LayerMask.NameToLayer("EnemyLayer"))
			//		//	SceneManager.LoadScene ("WinScene");
			//		//else
			//		//	SceneManager.LoadScene ("LoseScene");
					
			//	}
			//}
            if(!playerCharacter_Character.enabled)
            {
                StartCoroutine(GotoLoseScene());
            }
            
		}

        IEnumerator GotoLoseScene()
        {
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene("LoseScene");
        }

		// Update is called once per frame
		void Update () {
            respawnEnemyTime -= 1;
            if (respawnEnemyTime <= 0)
            {
                GenerateEnemy();
                respawnEnemyTime = MAX_RESPAWN_ENEMY_TIME;
            }

            if (players.Count <= 1)
            {
                GenerateEnemy();
                respawnEnemyTime = MAX_RESPAWN_ENEMY_TIME;
            }


            HandlePause();
            GameFinish();



        }


        int numPlayers = 0;
        void GenerateEnemy()
        {
            

            for( int i = players.Count - 1; i >= 0; --i)
            {
                if (!players[i].GetComponent<SwordsmanCharacter>().enabled)
                {
                    players.Remove(players[i]);
                }
            }

            numPlayers = players.Count;


            Debug.Log(numPlayers);

            if(numPlayers > 4)
            {
                return;
            }

            int name_idx = 0;
            int birth_idx = 0;
            //TODO: random name

            //GameObject newEnemy = Instantiate(Resources.Load(enemyName[name_idx]),birthPoint[birth_idx].position, Quaternion.identity) as GameObject;
            GameObject newEnemy = Instantiate(Resources.Load("AI-Di"), birthPoint[birth_idx].position, Quaternion.identity) as GameObject;
            newEnemy.tag = "Human";
            newEnemy.layer = LayerMask.NameToLayer("EnemyLayer");

            newEnemy.GetComponent<SwordsmanCharacter>().swordsmanStatus.setHP(30);
            newEnemy.GetComponent<SwordsmanCharacter>().swordsmanStatus.setMaxHP(30);

            players.Add(newEnemy);
        }
    }
}