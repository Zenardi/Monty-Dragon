using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        PlayerControl player;

        private void Start()
        {
            player = GetComponent<PlayerControl>();
        }

        public override void Use(GameObject target)
        {
            PlayAblitySound();
            player.GetComponent<HealthSystem>().Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
            PlayAbilityAnimation();

        }

    }

}