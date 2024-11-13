using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance { get; private set; }

    public event EventHandler OnPlayerTakeDamage;

    private int playerHealth;
    private int playerMaxHealth = 100;
    private void Awake()
    {
        Instance = this;
        //fájlból kell majd betölteni
        playerHealth = playerMaxHealth;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("taken damage");
        OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }


}
