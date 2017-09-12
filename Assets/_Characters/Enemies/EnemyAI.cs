using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 9f;

        enum State { Idle, Attacking, Patrolling, Chasing}
        State state = State.Idle;

        PlayerMovement player;
        Character character;
        float currentWeaponRange = 4f;
        float distanceToPlayer;

        void Start()
        {
            player = GameObject.FindObjectOfType<PlayerMovement>();
            character = GetComponent<Character>();
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeapon().GetMaxAttackRange();

            if(distanceToPlayer > chaseRadius && state != State.Patrolling)
            {
                state = State.Patrolling;

                //stop what we are doing
                StopAllCoroutines();

                //start patrolling

            }

            if(distanceToPlayer <= chaseRadius && state != State.Chasing)
            {
                //stop what we are doing
                StopAllCoroutines();

                //chase player
                StartCoroutine(ChasePlayer());
            }

            if(distanceToPlayer <= currentWeaponRange && state != State.Attacking)
            {
                state = State.Attacking;
                //stop what we are doing
                StopAllCoroutines();

                //attack player
            }
        }

        IEnumerator ChasePlayer()
        {
            state = State.Chasing;
            while(distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0f, 0f, 255, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }

}