using UnityEngine;
using System.Collections;


namespace Swordsmanship
{
    public class EnemyGenerator : MonoBehaviour
    {
        public string[] enemyName;
        public Transform[] birthPoint;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        GameObject GenerateEnemy ()
        {
            int name_idx = 0;
            int birth_idx = 0;
            //TODO: random name

            //GameObject newEnemy = Instantiate(Resources.Load(enemyName[name_idx]),birthPoint[birth_idx].position, Quaternion.identity) as GameObject;
            GameObject newEnemy = Instantiate(Resources.Load("AI-Di"), birthPoint[birth_idx].position, Quaternion.identity) as GameObject;
            newEnemy.tag = "Human";
            newEnemy.layer = LayerMask.NameToLayer("EnemyLayer");
            

            return newEnemy;
        }
    }
}


