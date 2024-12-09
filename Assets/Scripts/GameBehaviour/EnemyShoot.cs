using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private EnemyAI enemyAi;
    private EnemyTypeScriptableObject enemyTypeSO;
    private float time;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform bulletStartLocation;
    [SerializeField] private Transform bulletStartLocation2;
    private bool shootingActive = false;

    private enum ShootingPoint
    {
        Left,
        Right
    }
    private ShootingPoint shootingPoint;

    private void Start()
    {
        enemyAi = GetComponent<EnemyAI>();
        enemyTypeSO = enemyAi.GetEnemyType();
        enemyAi.OnEnemyActivated += EnemyAi_OnEnemyActivated;
        enemyAi.OnEnemyDisabled += EnemyAi_OnEnemyDisabled;
        time = 0;
        shootingPoint = ShootingPoint.Left;
    }

    private void EnemyAi_OnEnemyDisabled(object sender, System.EventArgs e)
    {
        shootingActive = false;
    }

    private void EnemyAi_OnEnemyActivated(object sender, System.EventArgs e)
    {
        shootingActive = true;
    }

    private void Update()
    {
        if (shootingActive && !GameOverUI.Instance.gameOver)
        {
            //lövés timer
            time += Time.deltaTime;
            if(time > enemyTypeSO.enemyShootSpeed)
            {
                Shoot();
                time = 0;
            }
        }
    }

    private void Shoot()
    {
        GameObject newEnemyBullet = null;
        if (!enemyTypeSO.twoFirePoints)
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
        newEnemyBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.enemyTypeSO.enemyBulletSpeed);
        newEnemyBullet.GetComponent<EnemyBullet>().SetBulletDamage(enemyTypeSO.enemyDamage);
    }
}
