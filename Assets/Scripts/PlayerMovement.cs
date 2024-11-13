using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public GameObject Lovedek;
    
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

    [SerializeField] private float LovSebesseg = 666.0f;

    [SerializeField] private float eletIdo = 5.0f;

    [SerializeField] private Transform bulletStartLocation;

    private const string POWERUP_TAG = "PowerUp";

    private bool PoweredUp = false;

    private float PowerUpTime = 15.0f;

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


        //ha pálya szélét érinti a hajó akkor teleportáljon a túloldalra
        if(transform.position.y > 6.9f)
        {
            float newPosY = transform.position.y * -1 + 0.1f;
            transform.position = new Vector2(transform.position.x, newPosY);
        }
        if (transform.position.y < -6.9f)
        {
            float newPosY = transform.position.y * -1 - 0.1f;
            transform.position = new Vector2(transform.position.x, newPosY);
        }
        if (PowerUpTime <= 15.0f)        //ameddig nem éri el a power-up time limitet számol felfelé
        {
            PowerUpTime += Time.deltaTime;
        }
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
        if (PoweredUp)      //Power-uppok
        {
            do
            {
                if (GetComponent<Powerups>().PowerUpID == 0)
                {
                    GameObject newBullet1 = Instantiate(Lovedek, bulletStartLocation.position, this.transform.rotation);
                    newBullet1.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);

                    /*GameObject newBullet2 = Instantiate(Lovedek, bulletStartLocation.position, Quaternion.Euler);
                    newBullet2.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);

                    GameObject newBullet3 = Instantiate(Lovedek, bulletStartLocation.position, );
                    newBullet3.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);*/
                }


            } while (PowerUpTime < 15.0f);
        }

        else       //sima lövedék
        {
            GameObject newBullet = Instantiate(Lovedek, bulletStartLocation.position, this.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == POWERUP_TAG)
        {
            PowerUpTime = 0.0f;
            PoweredUp = true;
        }
    }

}
