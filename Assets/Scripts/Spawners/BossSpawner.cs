using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner Instance { get; private set; }

    [SerializeField] private EnemyTypeScriptableObject Boss1SO;
    [SerializeField] private EnemyTypeScriptableObject Boss2SO;
    [SerializeField] private EnemyTypeScriptableObject Boss3SO;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EnemySpawner.Instance.OnEnemiesCleared += EnemySpawner_OnEnemiesCleared;
    }

    private void EnemySpawner_OnEnemiesCleared(object sender, System.EventArgs e)
    {
        //ha meghalt az összes enemy akkor spawnol a boss
        SpawnStageBoss(StageManager.Instance.currentStage);
    }

    private void SpawnStageBoss(int stage)
    {
        EnemyTypeScriptableObject Boss = null;
        //eldönti melyik boss spawnoljon
        if (stage == 1) Boss = Boss1SO;
        if (stage == 2) Boss = Boss2SO;
        if (stage == 3) Boss = Boss3SO;

        Vector3 bossPos = PlayerMovement.Instance.transform.position + new Vector3(18f, 0f, 0f);
        bossPos.y = 0f;
        GameObject newBoss = Instantiate(Boss.enemyPrefab, bossPos, Quaternion.identity);
        newBoss.transform.localScale = new Vector2(0.16f, 0.16f);
        newBoss.transform.rotation = Quaternion.Euler(0, 0, 90);
        newBoss.GetComponent<BossAI>().SetBossType(Boss);

    }
}
