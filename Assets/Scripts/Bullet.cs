using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletDamage = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyAI>().TakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Boundary")
        {
            Destroy(this.gameObject);
        }
    }

 }
