﻿using RPG.Characters;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] Image healthBar;
    [SerializeField] AudioClip[] damageSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] float deathVanishSeconds = 2f;


    const String DEATH_TRIGGER = "Death";

    float currentHealthPoints;
    Animator animator;
    AudioSource audioSource;
    Character characterMovement;

    public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }


    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterMovement = GetComponent<Character>();
        currentHealthPoints = maxHealthPoints;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateHealthBar();
	}

    private void UpdateHealthBar()
    {
        if(healthBar)
        {
            healthBar.fillAmount = healthAsPercentage;
        }
    }

    public void TakeDamage(float damage)
    {
        bool characterDies = (currentHealthPoints - damage <= 0);
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        var clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
        audioSource.PlayOneShot(clip);
        
        if (characterDies)
        {
            StartCoroutine(KillCharacter());
        }

    }

    public void Heal(float points)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);

    }

    IEnumerator KillCharacter()
    {
        characterMovement.Kill();
        animator.SetTrigger(DEATH_TRIGGER);
        
        audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
        audioSource.Play();
        yield return new WaitForSecondsRealtime(audioSource.clip.length); //use audio clip later

        var playerComponent = GetComponent<PlayerControl>();
        if(playerComponent && playerComponent.isActiveAndEnabled)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            ///assume os enemy for now, reconsider on other NPC`s
            DestroyObject(gameObject, deathVanishSeconds);
        }


    }

}
