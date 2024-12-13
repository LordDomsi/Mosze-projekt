using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    public GameObject Lovedek;
    public GameObject Shield;

    private Rigidbody2D _rigidbody;

    private bool forwardMovement;
    private bool backwardMovement;

    public event EventHandler OnForwardPressed;
    public event EventHandler OnForwardStopped;
    public event EventHandler OnTurnLeft;
    public event EventHandler OnTurnRight;
    public event EventHandler OnStopTurn;

    private float _forog;

    [SerializeField] private float playerSpeed = 6f;
    public float sebesseg;
    [SerializeField] private float blackholeSebesseg = 3f;

    [SerializeField] private float forogSebesseg = 1.0f;

    [SerializeField] private float LovSebesseg = 666.0f;

    [SerializeField] private float eletIdo = 5.0f;

    [SerializeField] private Transform bulletStartLocation;

    private const string POWERUP_TAG1 = "PowerUp1";
    private const string POWERUP_TAG2 = "PowerUp2";
    private const string POWERUP_TAG3 = "PowerUp3";
    private const string POWERUP_TAG4 = "PowerUp4";

    private int PowerUpType = 0;
    public int maxShield = 3;
    public int maxHeal = 30;

    private bool PoweredUp = false;
    public bool shielded = false;
    private bool speedBoost = false;

    [SerializeField] private float PowerUpTimeLimit;
    [SerializeField] private float SpeedBoostLimit;
    public bool canMove = true;
    public bool canShoot = true;

    public event EventHandler<OnThreeWayPowerUpPickupArgs> OnThreeWayPowerUpPickup;
    public class OnThreeWayPowerUpPickupArgs : EventArgs
    {
        public float powerUpTimeLimit;
    }

    public bool legacyMovement = false;
    [SerializeField] private Transform cursor;
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorPosition;
    [SerializeField] private float speed;
    [SerializeField] private float blackholeSpeed;
    [SerializeField] private float defaultSpeed;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LocatorSpawner.Instance.OnLocatorPickup += LocatorSpawner_OnLocatorPickup;
        StageManager.Instance.OnStageInit += StageManager_OnStageInit;
        _rigidbody = GetComponent<Rigidbody2D>();
        if (!legacyMovement)
        {
            cursor.gameObject.SetActive(true);
            cursorPosition = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
            Cursor.SetCursor(cursorTexture, cursorPosition, CursorMode.Auto);
        }
        canMove = true;
        canShoot = true;
    }

    private void StageManager_OnStageInit(object sender, EventArgs e)
    {
        speed = defaultSpeed;
    }

    private void LocatorSpawner_OnLocatorPickup(object sender, EventArgs e)
    {
        speed = blackholeSpeed;
        StopPlayer();
    }

    private void Update()
    {
        if (legacyMovement == false && canMove)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector3 direction = (mousePosition - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle - 90, Time.deltaTime * 2f);
            transform.rotation = Quaternion.Euler(0, 0, (currentAngle));
            float mouseDistance = Vector2.Distance(transform.position, mousePosition);
            Vector3 targetPoint = transform.position + transform.up * mouseDistance;
            cursor.position = targetPoint;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (speedBoost)
                {
                    _rigidbody.AddForce(this.transform.up * speed * 1.2f * Time.deltaTime);
                    OnForwardPressed?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _rigidbody.AddForce(this.transform.up * speed * Time.deltaTime);
                    OnForwardPressed?.Invoke(this, EventArgs.Empty);
                }
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                OnForwardStopped?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (speedBoost)
                {
                    _rigidbody.AddForce(-this.transform.up * (speed / 2) * 1.2f * Time.deltaTime);
                }
                else
                {
                    _rigidbody.AddForce(-this.transform.up * speed / 2 * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (speedBoost)
                {
                    _rigidbody.AddForce(-this.transform.right * (speed / 1.25f) * 1.2f * Time.deltaTime);
                }
                else
                {
                    _rigidbody.AddForce(-this.transform.right * speed / 1.25f * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (speedBoost)
                {
                    _rigidbody.AddForce(this.transform.right * (speed / 1.25f) * 1.2f * Time.deltaTime);
                }
                else
                {
                    _rigidbody.AddForce(this.transform.right * speed / 1.25f * Time.deltaTime);
                }
            }

        }
        if (canMove && legacyMovement == true)
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
        if (canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) { Shoot(); AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.PLAYER_SHOOT); }
        }



        //ha pálya szélét érinti a hajó akkor teleportáljon a túloldalra
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

    private IEnumerator SpeedBoostTimer()
    {
        yield return new WaitForSeconds(SpeedBoostLimit);
        speedBoost = false;
    }

    private void Shoot()
    {
        if (PoweredUp)      //three-way shot
        {
            GameObject newBullet1 = Instantiate(Lovedek, bulletStartLocation.position, this.transform.rotation);
            newBullet1.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);

            GameObject newBullet2 = Instantiate(Lovedek, bulletStartLocation.position, Quaternion.Euler(0, 0, 20) * this.transform.rotation);
            newBullet2.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, 20) * this.transform.up * this.LovSebesseg);

            GameObject newBullet3 = Instantiate(Lovedek, bulletStartLocation.position, Quaternion.Euler(0, 0, -20) * this.transform.rotation);
            newBullet3.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, -20) * this.transform.up * this.LovSebesseg);

        }

        else       //sima lövedék
        {
            GameObject newBullet = Instantiate(Lovedek, bulletStartLocation.position, this.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.LovSebesseg);
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)      //1 three-way, 2 speed, 3 shield
    {
        if (collision.gameObject.tag == POWERUP_TAG1)
        {
            StartCoroutine(PowerUpTimer());
            OnThreeWayPowerUpPickup?.Invoke(this, new OnThreeWayPowerUpPickupArgs { powerUpTimeLimit = PowerUpTimeLimit });
            PoweredUp = true;
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.POWERUP);
        }
        else if (collision.gameObject.tag == POWERUP_TAG2)
        {
            StartCoroutine(SpeedBoostTimer());
            speedBoost = true;
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.POWERUP);
        }
        else if (collision.gameObject.tag == POWERUP_TAG3)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.POWERUP);
            if (shielded == false)
            {
                EnableShield();
            }
        }
        else if (collision.gameObject.tag == POWERUP_TAG4)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.POWERUP);
            PlayerHealthManager.Instance.Heal(maxHeal);
        }
    }

    public void StopPlayer()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = 0f;
    }

    public void EnableShield()
    {
        Shield.gameObject.SetActive(true);
        shielded = true;
    }

    public void DisableShield()
    {
        Shield.gameObject.SetActive(false);
        shielded = false;
    }

    public void IncreaseMaxShield(int plusShield)
    {
        maxShield += plusShield;
    }
}
