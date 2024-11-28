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

    private void OnEnable()
    {
        shieldHealth = 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ASTEROID_TAG || collision.gameObject.tag == ENEMY_TAG || collision.gameObject.tag == ENEMY_BULLET_TAG)
        {
            ReduceShieldHealth();
        }
    }

    public void ReduceShieldHealth()
    {
        shieldHealth--;
        if (shieldHealth <= 0)
        {
            PlayerMovement.Instance.DisableShield();
        }
    }

    
}
