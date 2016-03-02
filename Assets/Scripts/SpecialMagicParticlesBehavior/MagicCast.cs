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
			rb = gameObject.GetComponent<Rigidbody> ();
        }

        // Update is called once per frame
        void Update()
		{
			switch (type) {
			case MagicCastType.ConstantMoving:
                    //transform.position += moveSpeed * dir;
				break;
			case MagicCastType.AccelerateMoving:
				rb.AddForce (transform.forward * acceleration);
				break;
			case MagicCastType.Tracking:
				break;
			}
		}

        public void InitMagicCast_Tracking(GameObject launcher, Transform target)
        {
            this.launcher = launcher;
            this.type = MagicCastType.Tracking;
            this.target = target;


            switch (type)
            {
                case MagicCastType.ConstantMoving:
                case MagicCastType.AccelerateMoving:
                    dir = target.position - transform.position;
                    dir.Normalize();
                    break;
                case MagicCastType.Tracking:
                    break;
            }
        }

        public void InitMagicCast_Straight(GameObject launcher, MagicCastType type, Vector3 direction)
        {
            this.launcher = launcher;
            this.type = type;

            dir = direction;
            dir.Normalize();

			rb.AddForce (dir * thrust);
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

