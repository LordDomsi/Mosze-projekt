using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Bullet Lovedek;
    
    private Rigidbody2D _rigidbody;

    private bool _raketa;

    private float _forog;

    [SerializeField] private float sebesseg = 1.0f;

    [SerializeField] private float forogSebesseg = 1.0f;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        _raketa = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _forog = 1.0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            _forog = -1.0f;
        } else
        {
            _forog = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space)) { Shoot(); }

    }

    private void FixedUpdate()
    {
        if (_raketa)
        {
            _rigidbody.AddForce(this.transform.up * this.sebesseg);
        }

        if (_forog != 0.0f)
        {
            _rigidbody.AddTorque(_forog * this.forogSebesseg);
        }

    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.Lovedek, this.transform.position, this.transform.rotation);
        bullet.shoot(this.transform.up);
    }

}
