using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{   //player �leterej��rt felel�s script
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
        playerHealth = SaveManager.Instance.saveData.playerData.currentHealth;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G)) // tesztel�s c�lj�b�l l�trehozott GodMode
        {
            //ToggleGodMode();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!godMode)
        {
            if (playerHealth <= 0 && exploded == false) //ha meghal a player
            {
                exploded = true;
                StartCoroutine(PlayerDeath());
            }
            else
            {
                playerHealth -= damage;

                AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.PLAYER_HIT); //hangeffekt
                PlayerMovement.Instance.gameObject.GetComponent<HitIndicator>().Hit(); //hit anim�ci�
                OnPlayerTakeDamage?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Heal(int health)
    {
        playerHealth += health;  
        if (playerHealth >= 100) // t�l healel�s miatt
        {
            playerHealth = 100;
        }
        OnPlayerHeal?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator PlayerDeath()
    {
        PlayerMovement.Instance.canMove = false; //player nem tud mozogni �s l�ni
        PlayerMovement.Instance.canShoot = false;

        Instantiate(explosionPrefab, this.transform.position , Quaternion.identity); //robban�s effekt
        AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.EXPLOSION); //robban�s hang
        spriteRenderer.sprite = explosionTexture; //felrobbant text�r�ra v�lt
        
        SaveManager.Instance.AddScoreToLeaderboard(ScoreManager.Instance.GetPlayerScore()); //friss�ti a leaderboardot

        yield return new WaitForSeconds(timeTillGameOver); //ne egyb�l jelenjen meg a game over screen
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }

    public void ToggleGodMode()
    { 
        godMode = !godMode;
    }


}
