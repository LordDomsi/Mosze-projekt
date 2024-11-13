using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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

    private const string POWERUP_TAG1 = "PowerUp1";
    private const string POWERUP_TAG2 = "PowerUp2";
    private const string POWERUP_TAG3 = "PowerUp3";

    private int PowerUpType = 0;

    private bool PoweredUp = false;

    [SerializeField] private float PowerUpTimeLimit;
    public bool canMove = true;

    public event EventHandler<OnThreeWayPowerUpPickupArgs> OnThreeWayPowerUpPickup;
    public class OnThreeWayPowerUpPickupArgs: EventArgs
    {
        public float powerUpTimeLimit;
    }


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
        if (canMove)
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
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                _forog = -1.0f;
                OnTurnRight?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _forog = 0.0f;
                OnStopTurn?.Invoke(this, EventArgs.Empty);
            }
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

    private IEnumerator PowerUpTimer()
    {
        yield return new WaitForSeconds(PowerUpTimeLimit);
        PoweredUp = false;
    }

    private void Shoot()
    {
        if (PoweredUp)      //Power-uppok
        {
                if (PowerUpType == 1)       //three-way shot
                {
                    GameObject newBullet1 = Instantiate(Lovedek, bulletStartLocation.position, this.transform.rotation);
                    newBullet1.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);

                    GameObject newBullet2 = Instantiate(Lovedek, bulletStartLocation.position, Quaternion.Euler(0, 0, 30) * this.transform.rotation);
                    newBullet2.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, 30) * this.transform.up * this.LovSebesseg);

                    GameObject newBullet3 = Instantiate(Lovedek, bulletStartLocation.position, Quaternion.Euler(0, 0, -30) * this.transform.rotation);
                    newBullet3.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, -30) * this.transform.up * this.LovSebesseg);
                }


        }

        else       //sima lövedék
        {
            GameObject newBullet = Instantiate(Lovedek, bulletStartLocation.position, this.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);
        }

        

    }

    private void OnCollisionEnter2D(Collision2D collision)      //1 three-way, 2 turbo, 3 WIP
    {
        if (collision.gameObject.tag == POWERUP_TAG1)
        {
            StartCoroutine(PowerUpTimer());
            OnThreeWayPowerUpPickup?.Invoke(this, new OnThreeWayPowerUpPickupArgs { powerUpTimeLimit = PowerUpTimeLimit} );
            PoweredUp = true;
            PowerUpType = 1;
        } 
        else if(collision.gameObject.tag == POWERUP_TAG2)
        {
            StartCoroutine(PowerUpTimer());
            PoweredUp = true;
            PowerUpType = 2;
        }
        else if (collision.gameObject.tag == POWERUP_TAG3)
        {
            StartCoroutine(PowerUpTimer());
            PoweredUp = true;
            PowerUpType = 3;
        }
    }

    public void StopPlayer()
    {
        _rigidbody.velocity = Vector2.zero;
    }

}
