using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance { get; private set; }

    public event EventHandler OnPlayerTakeDamage;
    public event EventHandler OnPlayerDeath;

    [SerializeField] private GameObject explosionPrefab;

    private int playerHealth;
    private int playerMaxHealth = 100;

    private float timeTillGameOver = 1f;

    public bool exploded = false;

    [SerializeField] private Sprite explosionTexture;
    [SerializeField] private Sprite defaultTexture;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultTexture;
        playerHealth = SaveManager.Instance.saveData.currentHealth;
    }

    public void TakeDamage(int damage)
    { 
        if (playerHealth <= 0  && exploded == false)
        {
            exploded = true;
            StartCoroutine(PlayerDeath());
        }
        else
        {
            playerHealth -= damage;
            PlayerMovement.Instance.gameObject.GetComponent<HitIndicator>().Hit();
            OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }

    private IEnumerator PlayerDeath()
    {
        Instantiate(explosionPrefab, this.transform.position , Quaternion.identity);
        spriteRenderer.sprite = explosionTexture;
        PlayerMovement.Instance.canMove = false;
        PlayerMovement.Instance.canShoot = false;
        yield return new WaitForSeconds(timeTillGameOver);
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }


}
