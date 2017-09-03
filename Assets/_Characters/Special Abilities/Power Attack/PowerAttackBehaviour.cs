using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(AbilityUseParams useParams)
        {
            PlayAblitySound();

            DealDamage(useParams);
            PlayParticleEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float dmgToDeal = useParams.baseDamage + (config as PowerAttackConfig).GetExtraDamage();
            useParams.target.TakeDamage(dmgToDeal);
        }
    }
}
