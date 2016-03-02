using UnityEngine;
using System.Collections;

namespace Swordsmanship
{
    public enum MagicCastType
    {
        ConstantMoving,
        AccelerateMoving,
        Tracking,
		RandomMoving
    };

    public class MagicCast : MonoBehaviour
    {
        // TODO:
        // Speical move idx


        //[SerializeField]
        MagicCastType type;

        //[SerializeField]
        //float y_rotationTrackingSpeed;
//        [SerializeField]
//        float rotationForce;

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

        GameObject launcher;

		Rigidbody rigidBody;

		EnemyUtil enemyUtil;
		private float smoothTime = 0.3f;



        // Use this for initialization
        void Start()
		{
			enemyUtil = GameObject.Find ("EnemyUtilityObj").GetComponent<EnemyUtil> ();
        }
			
		void Update()
		{
			
            switch(type)
            {
                case MagicCastType.ConstantMoving:
				    break;
			    case MagicCastType.AccelerateMoving:
				    rigidBody.AddForce (transform.forward * acceleration);
                    break;
                case MagicCastType.Tracking:
                    break;
			case MagicCastType.RandomMoving:
					rigidBody.AddForce(sampleDirectionOnCircle()*10);
					break;
				default:
					break;
            }

		}
			
        
		private void GetRigidBodyReference()
        {
			rigidBody = gameObject.GetComponent<Rigidbody>();
        }

//        public void InitMagicCast_Tracking(GameObject launcher, Transform target)
//        {
//            GetRigidBodyReference();
//            this.launcher = launcher;
//            this.type = MagicCastType.Tracking;
//            this.target = target;
//
//            rigidBody.AddForce(transform.right * 50);
//        }

//        public void InitMagicCast_Straight(GameObject launcher, MagicCastType type, Vector3 direction)
//        {
//            GetRigidBodyReference();
//            this.launcher = launcher;
//            this.type = type;
//
//            dir = direction;
//            dir.Normalize();
//            
//            rigidBody.AddForce ((dir + 0.5f * (UnityEngine.Random.value - 0.5f) * transform.right) * thrust);
//        }

		public void InitMagicCast(GameObject launcher, MagicCastType type)
		{
			GetRigidBodyReference();
			this.launcher = launcher;
			this.type = type;

			Vector3 initDirection = launcher.transform.forward.normalized; 

			if (type == MagicCastType.RandomMoving)
				rigidBody.AddForce (getRandomDirection (initDirection) / 4.0f);
			else
				rigidBody.AddForce (getRandomDirection (initDirection));
		}

		Vector3 getRandomDirection(Vector3 dir)
		{
			Vector3 right = launcher.transform.right.normalized;

			return (dir + 0.5f * (UnityEngine.Random.value - 0.5f) * right) * thrust;
		}

		Vector3 sampleDirectionOnCircle()
		{
			float u = UnityEngine.Random.value;
			float v = UnityEngine.Random.value;

			float r = Mathf.Sqrt (u);
			float theta = v * Mathf.PI * 2;
			return new Vector3 (r * Mathf.Cos (theta), 0, r * Mathf.Sin (theta));

		}

		public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == launcher)
            {
                return;
            }

            if(other.tag == "Wall")
            {
                HitDestroy();
            }
            else if(other.tag == "Attackable")
            {
                //be Knocked back
                Vector3 dir =  other.gameObject.transform.position - transform.position;
                //dir.y = 0;
                other.GetComponent<Rigidbody>().AddForce(knockBackForce * dir.normalized);
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

