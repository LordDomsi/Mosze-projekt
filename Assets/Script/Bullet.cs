using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private Rigidbody2D _rigidbody;

    [SerializeField] private float LovSebesseg = 666.0f;

    [SerializeField] private float eletIdo = 5.0f;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void shoot(Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.LovSebesseg);

        Destroy(this.gameObject, this.eletIdo);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            Destroy(this.gameObject);
        }
    }

 }
