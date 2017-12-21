using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using RPG.Core;

public class AreaEffectBehaviour : AbilityBehaviour
{

    public override void Use(GameObject target)
    {
        PlayAblitySound();
        DealRadialDamage();
        PlayParticleEffect();
        PlayAbilityAnimation();
    }



    private void DealRadialDamage()
    {
        //static spehere for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            (config as AreaEffectConfig).GetRadius(),
            Vector3.up,
             (config as AreaEffectConfig).GetRadius());

        foreach (var item in hits)
        {
            var damageble = item.collider.gameObject.GetComponent<HealthSystem>();
            bool hitPlayer = item.collider.gameObject.GetComponent<PlayerControl>();
            if (damageble != null && !hitPlayer)
            {
                float damageToDeal =  (config as AreaEffectConfig).GetDamageToEachTarget();
                damageble.TakeDamage(damageToDeal);
            }
        }
    }

}
