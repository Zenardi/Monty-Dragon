﻿using RPG.Characters;
//using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particleObject = null;
        [SerializeField] AudioClip[]  audioClips = null;
        [SerializeField] AnimationClip abilityAnimation;


        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToAttachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToAttachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlePrefab()
        {
            return particleObject;
        }

        public AudioClip GetRandimAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        public AnimationClip GetAbilityAnimation()
        {
            return abilityAnimation;
        }
    }
}