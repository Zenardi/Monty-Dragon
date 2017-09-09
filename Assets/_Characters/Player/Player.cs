using RPG.CameraUI;
using RPG.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        
        [SerializeField] float baseDamage = 10;
        [SerializeField] Weapon weaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChange = 0.1f;
        [SerializeField] float criticalHitMult = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle = null;


        //For debug
        [SerializeField] AbilityConfig[] abilities;

        const String ANIM_ATTACK_TRIGGER = "Attack";
        const String DEFAULT_ATTACK = "Default Attack";

        CameraRaycaster cameRaraycaster = null;
        float lastHitTime = 0f;

        Enemy enemy = null;

        GameObject WeaponObject;

        private void Start()
        {
            RegisterMouseClick();
            PutWeaponInHand(weaponConfig);
            //SetAttackAnimation();
            AttachInitialAbilities();
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }

        }

        private void Update()
        {
            var healthPercentage = GetComponent<HealthSystem>().healthAsPercentage;
            if(healthPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 0; keyIndex < abilities.Length; keyIndex++)
            {
                if(Input.GetKeyDown(keyIndex.ToString()))
                {
                    AttemptSpecialAbility(keyIndex);
                }
            }
        }




        private GameObject RequestDominantHand()
        {
            var dominantHand = GetComponentsInChildren<DominantHand>();
            int numberDominantHand = dominantHand.Length;

            Assert.AreNotEqual(numberDominantHand, 0, "No dominantHand found on player, add one");
            Assert.IsFalse(numberDominantHand > 1, "Multiple dominandHand on Player, remove one");

            return dominantHand[0].gameObject;
        }

        private void RegisterMouseClick()
        {
            cameRaraycaster = FindObjectOfType<CameraRaycaster>();
            cameRaraycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if(Input.GetMouseButton(0) && IsTargetInRange(enemyToSet.gameObject))
            {
                AttackTarget();
            }
            else if(Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0);
            }
        }

        private void AttemptSpecialAbility(int index)
        {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[index].GetEnergyCost();

            if(energyComponent.IsEnergyAvailable(energyCost)) //TODO read form SO (scriptable obj)
            {
                energyComponent.ConsumeEnergy(10f);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[index].Use(abilityParams);
                //Use the ability
            }
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > weaponConfig.GetMinTimeBetweenHits())
            {
                //SetAttackAnimation();
                //animator.SetTrigger(ANIM_ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) < criticalHitChange;
            float damageBeforeCritical = baseDamage + weaponConfig.GetAdditionalDamage();

            if (isCriticalHit)
            {
                criticalHitParticle.Play();
                return damageBeforeCritical * criticalHitMult;

            }
            else
                return damageBeforeCritical;
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponConfig.GetMaxAttackRange();
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            weaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(WeaponObject);
            WeaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            WeaponObject.transform.localPosition = this.weaponConfig.gripTransform.localPosition;
            WeaponObject.transform.localRotation = this.weaponConfig.gripTransform.localRotation;
        }

        public void TakeDamage(float damage)
        {
            throw new NotImplementedException();
        }
    }

}