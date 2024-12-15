using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerMovement : MonoBehaviour
{
    //A player irányításáért felelős script

    public static PlayerMovement Instance { get; private set; }

    private const string POWERUP_TAG1 = "PowerUp1";
    private const string POWERUP_TAG2 = "PowerUp2";
    private const string POWERUP_TAG3 = "PowerUp3";
    private const string POWERUP_TAG4 = "PowerUp4";

    public event EventHandler OnForwardPressed;
    public event EventHandler OnForwardStopped;
    public event EventHandler OnPlayerShoot;
    public event EventHandler OnSpeedBoostStart;
    public event EventHandler OnSpeedBoostEnd;

    public GameObject BulletPrefab; //lövedék
    public GameObject ShieldVisual;
    public GameObject SpeedBoostVisual;
    public GameObject TriShotVisual;
    [SerializeField] private Transform bulletStartLocation;
    [SerializeField] private Transform speedBoostEffect; //speed boost effektért felelős post processing gameobject

    private Rigidbody2D rb;

    //player statok
    [SerializeField] private float BulletSpeed = 666.0f;
    [SerializeField] private float speed;
    private const float DEFAULT_SHOOTSPEED = 0.5f;
    private float playerDamage = 1f;
    private float playerShootSpeed = 0.5f;
    private int maxShield = 3;
    public int maxHeal = 30;
    private float speedBoostMultiplier = 1.25f;

    private bool PoweredUp = false;
    public bool shielded = false;
    private bool speedBoost = false;

    [SerializeField] private float PowerUpTimeLimit;
    [SerializeField] private float SpeedBoostLimit;

    public bool canMove = true;
    public bool canShoot = true;
    public bool shootingCooldown = false;

    public event EventHandler<OnPowerupPickupEventArgs> OnThreeWayPowerUpPickup;
    public event EventHandler<OnPowerupPickupEventArgs> OnSpeedBoostPickup;
    public class OnPowerupPickupEventArgs : EventArgs
    {
        public float powerUpTimeLimit;
    }

    [SerializeField] private Transform sight;
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorPosition;
    private Vector3 mousePosition;
    private Vector3 direction;
    private Vector3 targetPoint;
    private float targetAngle;
    private float currentAngle;
    private float mouseDistance;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        //statok betöltése fájlból
        playerDamage = SaveManager.Instance.saveData.playerData.currentDamage;
        playerShootSpeed = SaveManager.Instance.saveData.playerData.currentAttackSpeed;
        speed = SaveManager.Instance.saveData.playerData.currentMovementSpeed;
        maxShield = SaveManager.Instance.saveData.playerData.currentMaxShield;
    }
    private void Start()
    {
        LocatorSpawner.Instance.OnLocatorPickup += LocatorSpawner_OnLocatorPickup;

        sight.gameObject.SetActive(true);
        cursorPosition = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorPosition, CursorMode.Auto);

        canMove = true;
        canShoot = true;
        StatsUI.Instance.UpdateStats();
    }

    private void LocatorSpawner_OnLocatorPickup(object sender, EventArgs e)
    {
        StopPlayer();
        if (LocatorSpawner.Instance.GetCurrentLocators() < 3)
        {
            maxShield++;
            playerDamage += 0.5f;
            playerShootSpeed -= 0.2f;
            speed += 25;
            StatsUI.Instance.UpdateStats();
        }
    }

    private void Update()
    {
        if (canMove) // mozgás
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (speedBoost){rb.AddForce(this.transform.up * speed * speedBoostMultiplier * Time.deltaTime);OnForwardPressed?.Invoke(this, EventArgs.Empty);}
                else{rb.AddForce(this.transform.up * speed * Time.deltaTime);OnForwardPressed?.Invoke(this, EventArgs.Empty);}
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                OnForwardStopped?.Invoke(this, EventArgs.Empty);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (speedBoost){rb.AddForce(-this.transform.up * (speed / 2) * speedBoostMultiplier * Time.deltaTime);}else{rb.AddForce(-this.transform.up * speed / 2 * Time.deltaTime);}
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                if (speedBoost){rb.AddForce(-this.transform.right * (speed / 1.25f) * speedBoostMultiplier * Time.deltaTime);}else{rb.AddForce(-this.transform.right * speed / 1.25f * Time.deltaTime);}
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                if (speedBoost){rb.AddForce(this.transform.right * (speed / 1.25f) * speedBoostMultiplier * Time.deltaTime);}else{rb.AddForce(this.transform.right * speed / 1.25f * Time.deltaTime);}
            }

        }
        if (canShoot && !shootingCooldown) //lövés
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) { Shoot(); AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.PLAYER_SHOOT); }
        }
    }
    
    private void LateUpdate()
    {
        if (canMove)
        { //célkereszt és a player forgatása
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            direction = (mousePosition - transform.position).normalized;
            targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle - 90, Time.deltaTime * 2f);
            transform.rotation = Quaternion.Euler(0, 0, (currentAngle));
            mouseDistance = Vector2.Distance(transform.position, mousePosition);
            targetPoint = transform.position + transform.up * mouseDistance;
            sight.position = targetPoint;
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

    private void Shoot()
    {
        OnPlayerShoot?.Invoke(this, EventArgs.Empty);
        if (PoweredUp)      //three-way shot
        {
            GameObject newBullet1 = Instantiate(BulletPrefab, bulletStartLocation.position, this.transform.rotation);
            newBullet1.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.BulletSpeed);

            GameObject newBullet2 = Instantiate(BulletPrefab, bulletStartLocation.position, Quaternion.Euler(0, 0, 15) * this.transform.rotation);
            newBullet2.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, 15) * this.transform.up * this.BulletSpeed);

            GameObject newBullet3 = Instantiate(BulletPrefab, bulletStartLocation.position, Quaternion.Euler(0, 0, -15) * this.transform.rotation);
            newBullet3.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, -15) * this.transform.up * this.BulletSpeed);

        }
        else       //sima lövedék
        {
            GameObject newBullet = Instantiate(BulletPrefab, bulletStartLocation.position, this.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.BulletSpeed);
        }
        StartCoroutine(ShootCooldown());
    }

    private void OnTriggerEnter2D(Collider2D collision)      //1 three-way, 2 speed, 3 shield, 4 heal
    {
        if (collision.gameObject.tag == POWERUP_TAG1)
        {
            StartCoroutine(PowerUpTimer());
            OnThreeWayPowerUpPickup?.Invoke(this, new OnPowerupPickupEventArgs { powerUpTimeLimit = PowerUpTimeLimit });
            PoweredUp = true;
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.POWERUP);
        }
        else if (collision.gameObject.tag == POWERUP_TAG2)
        {
            StartCoroutine(SpeedBoostTimer());
            OnSpeedBoostPickup?.Invoke(this, new OnPowerupPickupEventArgs { powerUpTimeLimit = SpeedBoostLimit });
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
            else
            {
                ShieldVisual.GetComponent<Shield>().ResetShield();
            }
        }
        else if (collision.gameObject.tag == POWERUP_TAG4)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.POWERUP);
            PlayerHealthManager.Instance.Heal(maxHeal);
        }
    }

    private IEnumerator ShootCooldown()
    {
        shootingCooldown = true;
        yield return new WaitForSeconds(playerShootSpeed);
        shootingCooldown = false;
    }

    private IEnumerator PowerUpTimer()
    {
        TriShotVisual.gameObject.SetActive(true);
        yield return new WaitForSeconds(PowerUpTimeLimit);
        PoweredUp = false;
        TriShotVisual.gameObject.SetActive(false);
    }

    private IEnumerator SpeedBoostTimer()
    {
        OnSpeedBoostStart?.Invoke(this, EventArgs.Empty);
        SpeedBoostVisual.gameObject.SetActive(true);
        speedBoostEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(SpeedBoostLimit);
        speedBoostEffect.gameObject.SetActive(false);
        speedBoost = false;
        SpeedBoostVisual.gameObject.SetActive(false);
        OnSpeedBoostEnd?.Invoke(this, EventArgs.Empty);
    }

    public void StopPlayer()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }

    public void EnableShield()
    {
        ShieldVisual.gameObject.SetActive(true);
        shielded = true;
    }

    public void DisableShield()
    {
        ShieldVisual.gameObject.SetActive(false);
        shielded = false;
    }

    public float GetPlayerDamage()
    {
        return playerDamage;
    }

    public float GetPlayerSpeed()
    {
        return speed;
    }

    public int GetPlayerMaxShield()
    {
        return maxShield;
    }

    public float GetPlayerShootSpeed()
    {
        return playerShootSpeed;
    }

    public float GetPlayerDefaultShootSpeed()
    {
        return DEFAULT_SHOOTSPEED;
    }

    public void EnableSight()
    {
        sight.gameObject.SetActive(true);
    }

    public void DisableSight()
    {
        sight.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        LocatorSpawner.Instance.OnLocatorPickup -= LocatorSpawner_OnLocatorPickup;
    }
}
