using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager Instance { get; private set; }

    public event EventHandler OnPlayerTakeDamage;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerHeal;

    [SerializeField] private GameObject explosionPrefab;

    private int playerHealth;
    private int playerMaxHealth = 100;

    private float timeTillGameOver = 1f;

    public bool exploded = false;

    private bool godMode = false;

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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            ToggleGodMode();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!godMode)
        {
            if (playerHealth <= 0 && exploded == false)
            {
                exploded = true;
                StartCoroutine(PlayerDeath());
            }
            else
            {
                AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.PLAYER_HIT);
                playerHealth -= damage;
                PlayerMovement.Instance.gameObject.GetComponent<HitIndicator>().Hit();
                OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Heal(int health)
    {
        playerHealth += health;
        OnPlayerHeal?.Invoke(this, EventArgs.Empty);
        if (playerHealth >= 100) // túl healelés miatt
        {
            playerHealth = 100;
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
        AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.EXPLOSION);
        spriteRenderer.sprite = explosionTexture;
        PlayerMovement.Instance.canMove = false;
        PlayerMovement.Instance.canShoot = false;
        SaveManager.Instance.AddScoreToLeaderboard(ScoreManager.Instance.GetPlayerScore());
        yield return new WaitForSeconds(timeTillGameOver);
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleGodMode()
    { 
        godMode = !godMode;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            if (!PlayerMovement.Instance.shielded) { TakeDamage(collision.GetComponent<Asteroid>().damage); ScreenShakeFX.Instance.ShakeCamera(2f, 0.2f); collision.GetComponent<Asteroid>().TryPlayAudio(); }
        }
    }


}
