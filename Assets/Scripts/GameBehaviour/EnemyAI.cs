using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private EnemyTypeScriptableObject enemyTypeSO;
    private bool enemyActive = false;
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;
    private float enemyHealth;
    public event EventHandler OnEnemyActivated;
    public event EventHandler OnEnemyDisabled;
    [SerializeField] private GameObject explosionPrefab;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }
    private void Update()
    {
        // ha messze vannak még az ellenfelek akkor ne legyen aktív az ai
        float distance = this.transform.position.x - PlayerMovement.Instance.transform.position.x;
        if (distance < 20f)
        {
            enemyActive = true; 
            polygonCollider.enabled = true;
            OnEnemyActivated?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            enemyActive = false;
            polygonCollider.enabled = false;
            OnEnemyDisabled?.Invoke(this, EventArgs.Empty);
        }

        //ha aktív akkor mozogjon és nézzen a player felé
        if (enemyActive)
        {
            LookTowardsPlayer();
            MovementHandle(distance);
        }
    }

    public void TakeDamage(float playerBulletDamage)
    {
        enemyHealth = enemyHealth - playerBulletDamage; //hp update

        AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.ENEMY_HIT); //hangeffekt
        EnemyHealthBar enemyHealthBar = GetComponent<EnemyHealthBar>(); //hp bar updateelése
        enemyHealthBar.UpdateHealthBar(enemyHealth, enemyTypeSO.enemyHealth); 
        this.gameObject.GetComponent<HitIndicator>().Hit(); //hit animáció
        
        if (enemyHealth <= 0) // ha meghal az enemy
        {
            ScoreManager.Instance.IncreasePlayerScore(enemyTypeSO.pointsWorth); //növeli a player scoret
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.EXPLOSION); //hangeffekt
            ScorePopup.Instance.Popup(this.transform, enemyTypeSO.pointsWorth); // megjeleníti hogy mennyi pontot kap a player
            Instantiate(explosionPrefab, this.transform.position, Quaternion.identity); // robbanás animáció
            Destroy(gameObject);
            EnemySpawner.Instance.DecreaseEnemyCount();
        }
    }

    public void SetEnemyType(EnemyTypeScriptableObject enemyType)
    {
        this.enemyTypeSO = enemyType;
        enemyHealth = enemyType.enemyHealth;
        EnemyHealthBar enemyHealthBar = GetComponent<EnemyHealthBar>();
        enemyHealthBar.UpdateHealthBar(enemyHealth, enemyTypeSO.enemyHealth);
    }

    private void LookTowardsPlayer()
    {
        Vector3 direction = PlayerMovement.Instance.transform.position - transform.position; // a player felé nézzen az enemy
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, enemyTypeSO.rotationSpeed * Time.deltaTime);
    }

    private void MovementHandle(float distance)
    {
        float speed;
        if (distance > 12f)
        {
            speed = -enemyTypeSO.enemySpeed; // balra mozog
        }
        else if (distance < 8f)
        {
            speed = enemyTypeSO.enemySpeed;  // jobbra mozog   
        }
        else
        {
            speed = 0; // megáll
        }
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }
    public EnemyTypeScriptableObject GetEnemyType()
    {
        return enemyTypeSO;
    }
}
