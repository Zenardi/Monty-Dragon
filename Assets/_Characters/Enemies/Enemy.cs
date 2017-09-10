using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour
    {
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

        PlayerMovement player = null;


        void Start()
        {
            player = GameObject.FindObjectOfType<PlayerMovement>();

        }

        void Update()
        {
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

        public void TakeDamage(float damage)
        {
            throw new NotImplementedException();
        }
    }

}