using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    EnemyTypeScriptableObject bossTypeSO;
    private Transform bossPosition;
    private bool initialMovement = true;
    private float bossHealth;
    public enum BossState
    {
        BasicAttack,
        SpecialAttack
    }

    public enum LookingTo
    {
        Up,
        Down
    }

    private enum ShootingPoint
    {
        Left,
        Right
    }
    private ShootingPoint shootingPoint;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform bulletStartLocation;
    [SerializeField] private Transform bulletStartLocation2;
    private Transform LookUp;
    private Transform LookDown;
    [SerializeField] private float basicAttackTime;
    [SerializeField] private float specialAttackTime;
    private float timerBasic;
    private float timerSpecial;
    private float rotationSpeed = 1f;
    private float shootingTimer;
    private float specialAttackShootingTimer;
    private BossState bossState;
    private LookingTo looking;


    private void Awake()
    {
        bossPosition = Camera.main.GetComponentInChildren<BossPosID>().transform;
        if (bossPosition != null)
        {
            LookUp = bossPosition.GetComponentInChildren<LookUpID>().transform;
            LookDown = bossPosition.GetComponentInChildren<LookDownID>().transform;
        }
        timerBasic = 0f;
        timerSpecial = 0f;
        shootingTimer = 0f;
        specialAttackShootingTimer = 0f;
    }

    private void Start()
    {
        bossState = BossState.BasicAttack;
        shootingPoint = ShootingPoint.Left;
        looking = LookingTo.Up;
    }

    private void Update()
    {
        //kamerába mozgás és kamera lock spawnoláskor
        if (initialMovement)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, bossPosition.position, bossTypeSO.enemySpeed * Time.deltaTime);
            float distance = this.transform.position.x - bossPosition.position.x;
            if (distance < 0.1f)
            {
                initialMovement = false;
            }
        }
        else
        {
            Vector2 pos = new Vector2(bossPosition.position.x, bossPosition.position.y);
            this.transform.position = pos;
        }

        //váltás a különbözõ attack módok között timerrrel
        switch (bossState)
        {
            case BossState.BasicAttack:
                timerBasic += Time.deltaTime;
                if(timerBasic > basicAttackTime)
                {
                    bossState = BossState.SpecialAttack;
                    timerBasic = 0f;
                }
                break;
            case BossState.SpecialAttack:
                timerSpecial += Time.deltaTime;
                if (timerSpecial > specialAttackTime)
                {
                    bossState = BossState.BasicAttack;
                    timerSpecial = 0f;
                }
                break;
        }

        if (bossState == BossState.BasicAttack) BasicAttack();
        if (bossState == BossState.SpecialAttack) SpecialAttack();
    }

    private void BasicAttack()
    {
        // a player felé nézzen a boss
        Quaternion targetRotation = LookTowardsObject(PlayerMovement.Instance.transform);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, bossTypeSO.rotationSpeed * Time.deltaTime);

        shootingTimer += Time.deltaTime;
        if (shootingTimer > bossTypeSO.enemyShootSpeed)
        {
            Shoot();
            shootingTimer = 0f;
        }
    }

    private void SpecialAttack()
    {
        //fel és le forog felváltva
        switch (looking)
        {
            case LookingTo.Up:
                Quaternion targetRotation1 = LookTowardsObject(LookUp);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation1, bossTypeSO.specialAttackRotationSpeed * Time.deltaTime);

                float angle1 = Quaternion.Angle(transform.rotation, targetRotation1);
                if(angle1 < 10f)
                {
                    looking = LookingTo.Down;
                }
                break;
            case LookingTo.Down:
                Quaternion targetRotation2 = LookTowardsObject(LookDown);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation2, bossTypeSO.specialAttackRotationSpeed * Time.deltaTime);
                float angle2 = Quaternion.Angle(transform.rotation, targetRotation2);
                if (angle2 < 10f)
                {
                    looking = LookingTo.Up;
                }
                break;
        }
        //külön timer a special attacknak
        specialAttackShootingTimer += Time.deltaTime;
        if(specialAttackShootingTimer > bossTypeSO.specialAttackShootSpeed)
        {
            specialAttackShootingTimer = 0f;
            Shoot();
        }
    }

    private Quaternion LookTowardsObject(Transform tf)
    {
        Vector3 direction = tf.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        return targetRotation;
    }

    private void Shoot()
    {
        GameObject newEnemyBullet = null;
        if (!bossTypeSO.twoFirePoints)
        {
            newEnemyBullet = Instantiate(enemyBulletPrefab, bulletStartLocation.position, this.transform.rotation);
            //a lespawnolt bulletnek átadjuk az enemy sebzését
        }
        else
        {
            //ha két lövési pontja van az enemynek akkor váltogat a kettõ között
            switch (shootingPoint)
            {
                case ShootingPoint.Left:
                    newEnemyBullet = Instantiate(enemyBulletPrefab, bulletStartLocation.position, this.transform.rotation);
                    shootingPoint = ShootingPoint.Right;
                    break;
                case ShootingPoint.Right:
                    newEnemyBullet = Instantiate(enemyBulletPrefab, bulletStartLocation2.position, this.transform.rotation);
                    shootingPoint = ShootingPoint.Left;
                    break;
            }
        }
        newEnemyBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.bossTypeSO.enemyBulletSpeed);
        newEnemyBullet.GetComponent<EnemyBullet>().SetBulletDamage(bossTypeSO.enemyDamage);
    }

    public void TakeDamage(float playerBulletDamage)
    {
        bossHealth = bossHealth - playerBulletDamage;
        EnemyHealthBar enemyHealthBar = GetComponent<EnemyHealthBar>();
        this.gameObject.GetComponent<HitIndicator>().Hit();
        enemyHealthBar.UpdateHealthBar(bossHealth, bossTypeSO.enemyHealth);
        if (bossHealth <= 0)
        {
            ScoreManager.Instance.IncreasePlayerScore(bossTypeSO.pointsWorth);
            LocatorSpawner.Instance.Spawn(this.transform);
            Destroy(gameObject);
        }
    }

    public void SetBossType(EnemyTypeScriptableObject bossTypeSO)
    {
        this.bossTypeSO = bossTypeSO;
        bossHealth = bossTypeSO.enemyHealth;
        EnemyHealthBar enemyHealthBar = GetComponent<EnemyHealthBar>();
        enemyHealthBar.UpdateHealthBar(bossHealth, bossTypeSO.enemyHealth);
    }
}
