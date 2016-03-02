using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public enum MagicCastType
    {
        ConstantMoving,
        AccelerateMoving,
        Tracking
    };

    public class MagicCast : MonoBehaviour
    {
        // TODO:
        // Speical move idx


        //[SerializeField]
        MagicCastType type;
        
        [SerializeField]
        float moveSpeed;

        [SerializeField]
        float y_rotationTrackingSpeed;

        [SerializeField]
        int damage;

        [SerializeField]
        float knockBackForce;

        [SerializeField]
        GameObject hitEffect;


        Transform target;

        Vector3 dir;

        GameObject launcher;



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            switch(type)
            {
                case MagicCastType.ConstantMoving:
                    transform.position += moveSpeed * dir;
                    break;
                case MagicCastType.AccelerateMoving:
                    // TODO
                    break;
                case MagicCastType.Tracking:
                    {
                        Vector3 dis = target.position - transform.position;
                        
                    }
                    break;
            }
        }


        public void InitMagicCast_Tracking(GameObject launcher, Transform target)
        {
            this.launcher = launcher;
            this.type = MagicCastType.Tracking;
            this.target = target;
        }

        public void InitMagicCast_Straight(GameObject launcher, MagicCastType type, Vector3 direction)
        {
            this.launcher = launcher;
            this.type = type;

            dir = direction;
            dir.Normalize();
            
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == launcher)
            {
                return;
            }


            if(other.tag == "Attackable")
            {
                HitDestroy();
            }
            else if(other.tag == "Human")
            {
                other.gameObject.GetComponent<SwordsmanCharacter>().BeHit(gameObject, knockBackForce);
                HitDestroy();
            }
        }


        void HitDestroy()
        {
            GameObject.Instantiate(hitEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
            Destroy(this);
        }


    }
}

