﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public PlayerOrder playerOrder;
    public string characterName;
    public Image characterImage;
    public int damage;
    //was stats

    public int maxHealth;
    public float maxSpecialTimer;

    public int currentHealth
    {
        get;
        set;
    }

    public float currentSpecialTimer
    {
        get;
        private set;
    }

    void Awake()
    {
        currentHealth = maxHealth;
        currentSpecialTimer = maxSpecialTimer;

        GameManager.Instance.LoadPlayerData(playerOrder, this);   // Always have this in awake to set the player data before game begins
    }

    private void Start()
    {
        //GameManager.Instance.LoadPlayerData(playerOrder, this);   // Always have this in awake to set the player data before game begins
    }

    void Update()
    {

        CurrentHealthBoundaries();

        SpecialTimerUpdate();
    }

    public void TakeDamage (int damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetSpecialTimer()
    {
        currentSpecialTimer = 0.0f;
    }

    public virtual void Die ()
    {
        // Die in some way
        // This is meant to be overwritten
        Debug.Log(transform.name + " died.");
        Destroy(gameObject);
    }

    private void CurrentHealthBoundaries()
    {
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    private void SpecialTimerUpdate()
    {
        if (currentSpecialTimer < maxSpecialTimer)
        {
            currentSpecialTimer += Time.deltaTime;
        }
        else if (currentSpecialTimer >= maxSpecialTimer)
        {
            currentSpecialTimer = maxSpecialTimer;
        }
    }
}
