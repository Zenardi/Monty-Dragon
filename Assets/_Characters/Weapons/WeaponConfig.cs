using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig : ScriptableObject
    {
        public Transform gripTransform;
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float timeBetweenCycles = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] float damageDelay = .5f;


        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public float GetDamageDelay()
        {
            return damageDelay;
        }

        public AnimationClip GetAnimClip()
        {
            RemoveAnimationEvent();
            return attackAnimation;
        }

        /// <summary>
        /// So that asset packs cannot cause crashes
        /// </summary>
        private void RemoveAnimationEvent()
        {
            if (attackAnimation != null)
                attackAnimation.events = new AnimationEvent[0];
            else
                Debug.Log("AttackAnimation NULL (WebConfig Script)");
        }

        public float GetTimeBetweenCycles()
        {
            return timeBetweenCycles;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }
    }

}