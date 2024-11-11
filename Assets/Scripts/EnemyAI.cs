using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private EnemyTypeScriptableObject enemyType;
    private bool enemyActive = false;
    private int direction = 1;
    private Rigidbody2D rb;
    private float rotationSpeed = 0.6f;
    private float enemyHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        float distance = this.transform.position.x - PlayerMovement.Instance.transform.position.x;
        if (distance < 20f) enemyActive = true; // ha messze vannak még az ellenfelek akkor ne legyen aktív az ai
        else enemyActive = false;

        if (enemyActive)
        {
            Vector3 direction = PlayerMovement.Instance.transform.position - transform.position; // a player felé nézzen az enemy
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle-90);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float speed;
            if (distance > 12f)
            {
                speed = -enemyType.enemySpeed; // balra mozog
            }
            else if (distance < 8f)
            {
                speed = enemyType.enemySpeed;  // jobbra mozog   
            }
            else
            {
                speed = 0; // megáll
            }
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    public void TakeDamage(float playerBulletDamage)
    {
        enemyHealth = enemyHealth - playerBulletDamage;
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyType(EnemyTypeScriptableObject enemyType)
    {
        this.enemyType = enemyType;
        enemyHealth = enemyType.enemyHealth;
    }
}
