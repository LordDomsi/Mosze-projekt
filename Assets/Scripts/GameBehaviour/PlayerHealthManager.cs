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
        playerHealth = SaveManager.Instance.saveData.currentHealth;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        PlayerMovement.Instance.gameObject.GetComponent<HitIndicator>().Hit();
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
