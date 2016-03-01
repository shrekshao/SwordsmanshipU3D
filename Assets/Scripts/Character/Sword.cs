using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public enum SwordState
    {
        Attack = 0,
        Block
    };

    public class Sword : MonoBehaviour
    {
        //TODO: Sword Status, attack, effect ...

        public GameObject master;  //user of the sword

        public SwordsmanCharacter swordsman;

        public SwordState sword_state;

        [SerializeField]
        GameObject hitEffect;

		[SerializeField]
		GameObject blockEffect;


        // Use this for initialization
        void Start()
        {
            sword_state = SwordState.Block;
        }

        // Update is called once per frame
        void Update()
        {

        }

        //void OnTriggerEneter(Collider other)
        //{
        //    if(other.tag == "Attackable" && other.gameObject != master)
        //    {
        //        Debug.Log("Attack Hit Enter!!!!!");
        //    }
        //}

        //void OnTriggerStay(Collider other)
        void OnTriggerEnter(Collider other)
        {
            if(sword_state == SwordState.Attack)
            {
                if (other.tag == "Sword" /*|| other.tag == "Shield"*/)
                {
                    // be blocked
                    GetComponent<Collider>().enabled = false;
					CreateBlockedEffect ();

                    swordsman.BeBlocked();
                    return;
                }

                if ((other.tag == "Attackable" || other.tag == "Human" || other.tag == "Player" ) && other.gameObject != master)
                {
                    // Hit human

                    GetComponent<Collider>().enabled = false;
                    //GameObject.Instantiate(Resources.Load("Particles/hitEffect"), transform.position, Quaternion.identity);
                    //GameObject.Instantiate(hitEffect, transform.position, Quaternion.identity);
                    CreateHitEffect();

                    swordsman.AttackCharacter(other);
                }
            }
            
        }

        void CreateHitEffect()
        {
            GameObject.Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

		void CreateBlockedEffect()
		{
			GameObject.Instantiate(blockEffect, transform.position, Quaternion.identity);
		}
        //void OnTriggerExit(Collider other)
        //{
        //    if (other.tag == "Attackable" && other.gameObject != master)
        //    {
        //        Debug.Log("Attack Hit Exit!!!!!");
        //    }
        //}
    }
}

