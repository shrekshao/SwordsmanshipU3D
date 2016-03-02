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

        //[SerializeField]
        //float y_rotationTrackingSpeed;
        [SerializeField]
        float rotationForce;

        [SerializeField]
        int damage;

        [SerializeField]
        float knockBackForce;

        [SerializeField]
        GameObject hitEffect;

		[SerializeField]
		float thrust;

		[SerializeField]
		float acceleration;

        Transform target;

        Vector3 dir;

        GameObject launcher;

		Rigidbody rb;
		
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
				    break;
			    case MagicCastType.AccelerateMoving:
				    rb.AddForce (transform.forward * acceleration);
                    break;
                case MagicCastType.Tracking:
                    {
                        Vector3 dis = target.position - transform.position;
                        dis.Normalize();
                        float s = 1.5f - Vector3.Dot(dis, rb.velocity.normalized);
                        rb.AddForce(s * dis * rotationForce);
                    }
                    break;
            }
        }

        public void GetRigidBodyReference()
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        public void InitMagicCast_Tracking(GameObject launcher, Transform target)
        {
            GetRigidBodyReference();
            this.launcher = launcher;
            this.type = MagicCastType.Tracking;
            this.target = target;

            rb.AddForce(transform.right * 50);
        }

        public void InitMagicCast_Straight(GameObject launcher, MagicCastType type, Vector3 direction)
        {
            GetRigidBodyReference();
            this.launcher = launcher;
            this.type = type;

            dir = direction;
            dir.Normalize();
            
            rb.AddForce ((dir + 0.3f * (UnityEngine.Random.value - 0.5f) * transform.right) * thrust);
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

