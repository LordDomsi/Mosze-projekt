using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float bulletDamage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boundary")
        {
            Destroy(this.gameObject);
        }
    }

    public void SetBulletDamage(float damage)
    {
        bulletDamage = damage;
    }
}
