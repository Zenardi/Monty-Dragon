using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        public float damageCaused { get; set; }
        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter;
        const float destroyDelay = .01f;
        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer)
            {
                //DamageIfDamageables(collision);
            }
        }

        internal float GetDefaultLauchSpeed()
        {
            return projectileSpeed;
        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }
    }

}