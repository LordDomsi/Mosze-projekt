using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private const string ASTEROID_TAG = "Asteroid";
    private const string ENEMY_TAG = "Enemy";
    private const string ENEMY_BULLET_TAG = "EnemyBullet";

    private Rigidbody2D _rigidbody;
    private float speed = 6f;
    private int shieldHealth;

    [SerializeField] private Material defaultShieldMaterial;
    [SerializeField] private Material hitShieldMaterial;
    [SerializeField] private SpriteRenderer shieldRenderer;

    private float hitAnimLength = 0.15f;


    private void OnEnable()
    {
        shieldRenderer.material = defaultShieldMaterial;
        shieldHealth = PlayerMovement.Instance.GetPlayerMaxShield();
        ShieldUI.Instance.UpdateVisual();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ASTEROID_TAG || collision.gameObject.tag == ENEMY_TAG || collision.gameObject.tag == ENEMY_BULLET_TAG)
        {
            ReduceShieldHealth();
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.PLAYER_HIT);
            if(shieldRenderer.gameObject.activeSelf)StartCoroutine(ShieldHitVisual());
            if (collision.gameObject.tag == ASTEROID_TAG) { Destroy(collision.gameObject); collision.gameObject.GetComponent<Asteroid>().TryPlayAudio(); ScreenShakeFX.Instance.ShakeCamera(2f, 0.2f); };
        }
    }

    public void ReduceShieldHealth()
    {
        shieldHealth--;
        ShieldUI.Instance.UpdateVisual();
        if (shieldHealth <= 0)
        {
            PlayerMovement.Instance.DisableShield();
        }
    }

    public int GetShieldHealth()
    {
        return shieldHealth;
    }

    private IEnumerator ShieldHitVisual()
    {
        shieldRenderer.material = hitShieldMaterial;
        yield return new WaitForSeconds(hitAnimLength);
        shieldRenderer.material = defaultShieldMaterial;
    }

}
