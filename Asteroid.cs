using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const string BULLET_TAG = "Bullet";
    private Rigidbody2D rigidBody;
    private float size;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitialForce();
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);    //megforgatja random szogben
    }


    //Ha t�l messze ker�l az asteroida a playert�l akkor kit�rl�dik
    private void LateUpdate()
    {
        float distance = Vector2.Distance(this.transform.position, PlayerMovement.Instance.transform.position);
        if (distance > 30f)
        {
            AsteroidSpawner.Instance.DecreaseAsteroidCount();
            Destroy(this.gameObject);
        }
    }

    public void InitialForce()
    {
        float angle = Random.Range(0f, 2 * Mathf.PI);
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (size == 0.16f)           //mekkora a meret, nagyobb a sebesseg
        {
            rigidBody.AddForce(direction * Random.Range(0.5f, 1f), ForceMode2D.Impulse);
        }
        else if (size == 0.08f)
        {
            rigidBody.AddForce(direction * Random.Range(1f, 2f), ForceMode2D.Impulse);
        }
        else
        {
            rigidBody.AddForce(direction * Random.Range(2f, 3f), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == BULLET_TAG)
        {
             if((this.size) > 0.04f)
             {
                 Vector2 pos = this.transform.position;
                 pos = pos + (Random.insideUnitCircle / 2);
                 
                 AsteroidSpawner.Instance.SpawnAsteroid(pos, size/2);
                 AsteroidSpawner.Instance.SpawnAsteroid(pos, size/2);
             }
            AsteroidSpawner.Instance.DecreaseAsteroidCount();
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (transform.position.y > 6.9f)
        {
            float newPosY = transform.position.y * -1 + 0.1f;
            transform.position = new Vector2(transform.position.x, newPosY);
        }
        if (transform.position.y < -6.9f)
        {
            float newPosY = transform.position.y * -1 - 0.1f;
            transform.position = new Vector2(transform.position.x, newPosY);
        }
    }

    public void SetSize(float size)
    {
        this.size = size;
    }
}
