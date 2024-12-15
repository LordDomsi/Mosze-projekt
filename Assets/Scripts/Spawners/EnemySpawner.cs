using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    //az enemyk adatait tároló sciptable objetek
    [SerializeField] private EnemyTypeScriptableObject StageOneEnemySO;
    [SerializeField] private EnemyTypeScriptableObject StageTwoEnemySO;
    [SerializeField] private EnemyTypeScriptableObject StageThreeEnemySO;

    //Két objektum által meghatározzot helyen spawnolnak az enemyk
    [SerializeField] private Transform SpawnAreaCorner1;
    [SerializeField] private Transform SpawnAreaCorner2;

    private int enemyCount;

    public event EventHandler OnEnemiesCleared;
    public event EventHandler OnEnemiesSpawned;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StageManager.Instance.OnStageInit += StageManager_OnStageInit;
        AudioManager.Instance.PlayMusic(AudioManager.Music_enum.GAME_THEME);
    }

    private void StageManager_OnStageInit(object sender, EventArgs e)
    {
        SpawnAllEnemies(); //stage váltásnál lespawnol mindent
    }

    public void SpawnAllEnemies()
    {
        enemyCount = 0;
        SpawnEnemiesByNumber(StageOneEnemySO);
        SpawnEnemiesByNumber(StageTwoEnemySO);
        SpawnEnemiesByNumber(StageThreeEnemySO);
        OnEnemiesSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void SpawnEnemiesByNumber(EnemyTypeScriptableObject enemy)
    {
        for (int i = 0; i < enemy.enemyCountBasedOnStage[StageManager.Instance.currentStage - 1]; i++) // scriptable objectben meghatározott mennyiségû enemyt spawnol
        {
            Vector2 spawnPos = GenerateSpawnPosition(SpawnAreaCorner1, SpawnAreaCorner2);
            GameObject newEnemy = Instantiate(enemy.enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.localScale = new Vector2(0.06f, 0.06f);
            newEnemy.transform.rotation = Quaternion.Euler(0, 0, 90);
            newEnemy.GetComponent<EnemyAI>().SetEnemyType(enemy); // beállítja az enemy típust az enemyAI scriptben hogy az tudja milyen statjai vannak
            enemyCount++;
        }
    }
    public Vector2 GenerateSpawnPosition(Transform corner1, Transform corner2) //spawn pozició generálása
    {
        Vector2 spawnPos = new Vector2(UnityEngine.Random.Range(corner1.position.x, corner2.position.x), UnityEngine.Random.Range(corner1.position.y, corner2.position.y));
        return spawnPos;
    }

    public void DecreaseEnemyCount()
    {
        enemyCount--;
        Debug.Log("Enemy Count On Level:" + enemyCount);
        if (enemyCount == 0)
        {
            OnEnemiesCleared?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnDestroy()
    {
        StageManager.Instance.OnStageInit -= StageManager_OnStageInit;
    }
}

