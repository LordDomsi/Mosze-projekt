using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public Bullet Lovedek;
    
    private Rigidbody2D _rigidbody;

    private bool forwardMovement;
    private bool backwardMovement;

    public event EventHandler OnForwardPressed;
    public event EventHandler OnForwardStopped;
    public event EventHandler OnTurnLeft;
    public event EventHandler OnTurnRight;
    public event EventHandler OnStopTurn;

    private float _forog;

    [SerializeField] private float sebesseg = 1.0f;

    [SerializeField] private float forogSebesseg = 1.0f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        forwardMovement = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            OnForwardStopped?.Invoke(this, EventArgs.Empty);
        }
        backwardMovement = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _forog = 1.0f;
            OnTurnLeft?.Invoke(this, EventArgs.Empty);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            _forog = -1.0f;
            OnTurnRight?.Invoke(this, EventArgs.Empty);
        } else
        {
            _forog = 0.0f;
            OnStopTurn?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetKeyDown(KeyCode.Space)) { Shoot(); }

    }

    private void FixedUpdate()
    {
        if (forwardMovement)
        {
            _rigidbody.AddForce(this.transform.up * this.sebesseg);
            OnForwardPressed?.Invoke(this, EventArgs.Empty);
        }
        
        if (backwardMovement) _rigidbody.AddForce(-this.transform.up * this.sebesseg / 2);
        if (_forog != 0.0f) _rigidbody.AddTorque(_forog * this.forogSebesseg);
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.Lovedek, this.transform.position, this.transform.rotation);
        bullet.shoot(this.transform.up);
    }

}
