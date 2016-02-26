using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public class Sword : MonoBehaviour
    {
        //TODO: Sword Status

        public GameObject master;  //user of the sword

        // Use this for initialization
        void Start()
        {

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

        void OnTriggerStay(Collider other)
        {
            if (other.tag == "Attackable" && other.gameObject != master)
            {
                Debug.Log("Attack Hit!!!!!");
                GameObject.Instantiate(Resources.Load("Particles/hitEffect"), transform.position, Quaternion.identity);
                GetComponent<Collider>().enabled = false;
            }
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

