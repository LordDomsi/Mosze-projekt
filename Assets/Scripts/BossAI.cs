using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    EnemyTypeScriptableObject bossTypeSO;

    private Transform bossPosition;

    private bool initialMovement = true;

    private float bossHealth;



    private void Awake()
    {
        bossPosition = Camera.main.GetComponentInChildren<BossPosID>().transform;
    }

    private void Update()
    {
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
    }


    public void SetBossType(EnemyTypeScriptableObject bossTypeSO)
    {
        this.bossTypeSO = bossTypeSO;
        bossHealth = bossTypeSO.enemyHealth;
        EnemyHealthBar enemyHealthBar = GetComponent<EnemyHealthBar>();
        enemyHealthBar.UpdateHealthBar(bossHealth, bossTypeSO.enemyHealth);
    }
}
