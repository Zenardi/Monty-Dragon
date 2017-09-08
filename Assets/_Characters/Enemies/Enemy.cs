using RPG.Core;
using UnityEngine;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float firingPeriodInSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;

        [SerializeField] GameObject projectileToUse = null;
        [SerializeField] GameObject projectileSocket = null;
        [SerializeField] float vecticalAimOffset = 1.0f;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);


        bool isAttacking = false;
        float currentHealthPoints;

        Player player = null;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }

        void Start()
        {
            player = GameObject.FindObjectOfType<Player>();
            
            currentHealthPoints = maxHealthPoints;
        }

        void Update()
        {
            //if(player.healthAsPercentage <= Mathf.Epsilon)
            //{
            //    StopAllCoroutines();
            //    Destroy(this);
            //}
            ////if (aiCharacterControl == null)
            ////    print("aiCharacterControl is null -- " + this.gameObject.name.ToString());
            //float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            //if (distanceToPlayer <= attackRadius && aiCharacterControl != null && !isAttacking)
            //{
            //    isAttacking = true;
            //    float randDelay = Random.Range(firingPeriodInSeconds - firingPeriodVariation, firingPeriodInSeconds + firingPeriodVariation);
            //    InvokeRepeating("FireProjectile", 0f, randDelay); //TOOD switch to coroutines
            //}

            //if (distanceToPlayer > attackRadius && aiCharacterControl != null)
            //{
            //    isAttacking = false;
            //    CancelInvoke();
            //}

            //if (distanceToPlayer <= chaseRadius && aiCharacterControl != null)
            //{
            //   // aiCharacterControl.SetTarget(player.transform);
            //}
            //else if (aiCharacterControl != null)
            //{
            //   // aiCharacterControl.SetTarget(transform);
            //}
        }

        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(this.gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLauchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.GetDefaultLauchSpeed();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = new Color(0f, 0f, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }

}