using RPG.CameraUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RPG.Characters
{
    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] Image energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] AudioClip outOfEnergy;

        float currentEnergyPoints;
        AudioSource audioSource;

        public float energyAsPercent { get { return currentEnergyPoints / maxEnergyPoints; } }

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            currentEnergyPoints = maxEnergyPoints;
            AttachInitialAbilities();
            UpdateEnergyBar();
        }

        private void Update()
        {
            if(currentEnergyPoints <= maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        public int GetNumberOfSpecialAbilities()
        {
            return abilities.Length;
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }
        
        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);

            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            energyBar.fillAmount = energyAsPercent;
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }

        }

        public void AttemptSpecialAbility(int index, GameObject target = null)
        {
            var energyComponent = GetComponent<SpecialAbilities>();
            var energyCost = abilities[index].GetEnergyCost();

            if (energyCost <= currentEnergyPoints) //TODO read form SO (scriptable obj)
            {
                ConsumeEnergy(energyCost);
                abilities[index].Use(target);
            }
            else
            {
                audioSource.PlayOneShot(outOfEnergy);
            }
        }
    }
}
