using RPG.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Core;

public class AreaEffectBehaviour : AbilityBehaviour
{

    public override void Use(AbilityUseParams useParams)
    {
        PlayAblitySound();
        DealRadialDamage(useParams);
        PlayParticleEffect();
    }



    private void DealRadialDamage(AbilityUseParams useParams)
    {
        //static spehere for targets
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            (config as AreaEffectConfig).GetRadius(),
            Vector3.up,
             (config as AreaEffectConfig).GetRadius());

        foreach (var item in hits)
        {
            var damageble = item.collider.gameObject.GetComponent<IDamageable>();
            bool hitPlayer = item.collider.gameObject.GetComponent<Player>();
            if (damageble != null && !hitPlayer)
            {
                float damageToDeal = useParams.baseDamage + (config as AreaEffectConfig).GetDamageToEachTarget();
                damageble.TakeDamage(damageToDeal);
            }
        }
    }

}
