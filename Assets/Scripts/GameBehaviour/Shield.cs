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

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ASTEROID_TAG || collision.gameObject.tag == ENEMY_TAG || collision.gameObject.tag == ENEMY_BULLET_TAG)
        {
            Destroy(this.gameObject);
            PlayerMovement.Instance.shielded = false;
        }
    }
}
