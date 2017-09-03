using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        Player player;

        private void Start()
        {
            player = GetComponent<Player>();
        }

        public override void Use(AbilityUseParams useParams)
        {
            PlayAblitySound();

            player.Heal((config as SelfHealConfig).GetExtraHealth());

            PlayParticleEffect();
        }

    }

}