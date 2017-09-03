using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected AbilityConfig config;
        const float PARTICLE_CLEAN_DELAY = 20f;

        public abstract void Use(AbilityUseParams useParams);

        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlePrefab();
            var particleObject = Instantiate(
                particlePrefab, 
                transform.position, 
                particlePrefab.transform.rotation);

            particleObject.transform.parent = transform;

            particleObject.GetComponent<ParticleSystem>().Play();

            StartCoroutine(DestroyPartcileWhenFinished(particleObject));
        }

        IEnumerator DestroyPartcileWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_DELAY);
            }

            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected void PlayAblitySound()
        {
            var abilitySound = config.GetRandimAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }
}
