using RPG.CameraUI;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
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
            CameraRaycaster cameRaraycaster = FindObjectOfType<CameraRaycaster>();
            cameRaraycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameRaraycaster.onMouseOverPotentiallyWalkable += OnMousePottentiallyWalkable;

        }

        private void OnMousePottentiallyWalkable(Vector3 destination)
        {
            if(Input.GetMouseButton(0))
            {
                weaponSystem.StopAttacking();
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

        private void OnMouseOverEnemy(EnemyAI enemy)
        {
            if(Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if(Input.GetMouseButton(0) && !IsTargetInRange(enemy.gameObject))
            {
                //Move and attack
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if(Input.GetMouseButtonDown(1) && IsTargetInRange(enemy.gameObject))
            {
                abilities.AttemptSpecialAbility(0, enemy.gameObject);
            }
            else if(Input.GetMouseButtonDown(1) && !IsTargetInRange(enemy.gameObject))
            {
                //move and power attack
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            
            while(!IsTargetInRange(target.gameObject))
            {
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
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