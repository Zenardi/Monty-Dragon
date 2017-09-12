using RPG.CameraUI;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {
        

        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChange = 0.1f;
        [SerializeField] float criticalHitMult = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle;
        


        CameraRaycaster cameRaraycaster = null;
        EnemyAI enemy = null;
        SpecialAbilities abilities;
        Character character;
        WeaponSystem weaponSystem;

        private void Start()
        {
            character = GetComponent<Character>(); 
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            RegisterForMouseEvents();

        }

        private void RegisterForMouseEvents()
        {
            cameRaraycaster = FindObjectOfType<CameraRaycaster>();
            cameRaraycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameRaraycaster.onMouseOverPotentiallyWalkable += OnMousePottentiallyWalkable;

        }

        private void OnMousePottentiallyWalkable(Vector3 destination)
        {
            if(Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        private void Update()
        {
            ScanForAbilityKeyDown();
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 0; keyIndex < abilities.GetNumberOfSpecialAbilities(); keyIndex++)
            {
                if(Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        private void OnMouseOverEnemy(EnemyAI enemyToSet)
        {
            this.enemy = enemyToSet;
            if(Input.GetMouseButton(0) && IsTargetInRange(enemyToSet.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }

        public void TakeDamage(float damage)
        {
            throw new NotImplementedException();
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

    }

}