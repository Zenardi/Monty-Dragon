using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : AbilityBehaviour
    {
        public override void Use(GameObject target)
        {
            PlayAblitySound();

            DealDamage(target);
            PlayParticleEffect();
        }

        private void DealDamage(GameObject target)
        {
            float dmgToDeal = (config as PowerAttackConfig).GetExtraDamage();
            target.GetComponent<HealthSystem>().TakeDamage(dmgToDeal);
        }
    }
}
