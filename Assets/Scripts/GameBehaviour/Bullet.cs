using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //bullet collisionért felelõs script
    [SerializeField] private float bulletDamage;

    private void Awake()
    {
        bulletDamage = PlayerMovement.Instance.GetPlayerDamage();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid" || collision.gameObject.tag == "Boundary" || collision.gameObject.tag == "BlackHole")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyAI>().TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossAI>().TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }

 }
