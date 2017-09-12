
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {

        [SerializeField] float baseDamage = 10;
        [SerializeField] WeaponConfig weaponConfig = null;

        GameObject WeaponObject;
        GameObject target;
        Animator animator;
        Character character;
        float lastHitTime = 0f;

        const String ANIM_ATTACK_TRIGGER = "Attack";
        const String DEFAULT_ATTACK = "Default Attack";

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<Character>();

            PutWeaponInHand(weaponConfig);
            SetAttackAnimation();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            weaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(WeaponObject);
            WeaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            WeaponObject.transform.localPosition = this.weaponConfig.gripTransform.localPosition;
            WeaponObject.transform.localRotation = this.weaponConfig.gripTransform.localRotation;
        }

        private void SetAttackAnimation()
        {
            var animatorOverrideController = character.GetOverrideController();
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = character.GetOverrideController();
            animatorOverrideController[DEFAULT_ATTACK] = weaponConfig.GetAnimClip();
        }

        private GameObject RequestDominantHand()
        {
            var dominantHand = GetComponentsInChildren<DominantHand>();
            int numberDominantHand = dominantHand.Length;

            Assert.AreNotEqual(numberDominantHand, 0, "No dominantHand found on player, add one");
            Assert.IsFalse(numberDominantHand > 1, "Multiple dominandHand on Player, remove one");

            return dominantHand[0].gameObject;
        }

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > weaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ANIM_ATTACK_TRIGGER);
                lastHitTime = Time.time;
            }
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            //todo use a repeat attack co-routine
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return weaponConfig;
        }

        private float CalculateDamage()
        {
            return baseDamage + weaponConfig.GetAdditionalDamage();
        }
    }

}