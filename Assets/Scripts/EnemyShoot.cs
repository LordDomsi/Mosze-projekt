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
    private bool shootingActive = false;
    private void Start()
    {
        enemyAi = GetComponent<EnemyAI>();
        enemyTypeSO = enemyAi.GetEnemyType();
        enemyAi.OnEnemyActivated += EnemyAi_OnEnemyActivated;
        enemyAi.OnEnemyDisabled += EnemyAi_OnEnemyDisabled;
        time = 0;
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
        if (shootingActive)
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
        GameObject newEnemyBullet = Instantiate(enemyBulletPrefab, bulletStartLocation.position, this.transform.rotation);
        newEnemyBullet.GetComponent<Rigidbody2D>().AddForce(this.transform.up * this.enemyTypeSO.enemyBulletSpeed);
        newEnemyBullet.GetComponent<EnemyBullet>().SetBulletDamage(enemyTypeSO.enemyDamage); //a lespawnolt bulletnek átadjuk az enemy sebzését
    }
}
