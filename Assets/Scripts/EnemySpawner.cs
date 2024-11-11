using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //az enemyk adatait tároló sciptable objetek
    [SerializeField] private EnemyTypeScriptableObject StageOneEnemySO;
    [SerializeField] private EnemyTypeScriptableObject StageTwoEnemySO;
    [SerializeField] private EnemyTypeScriptableObject StageThreeEnemySO;

    //Két objektum által meghatározzot helyen spawnolnak az enemyk
    [SerializeField] private Transform SpawnAreaCorner1;
    [SerializeField] private Transform SpawnAreaCorner2;

    private int currentStage = 1;

    //Ez nem starton lesz hanem amikor staget váltunk csak az még nincs megírva
    private void Start()
    {
        SpawnEnemiesByNumber(StageOneEnemySO);
        SpawnEnemiesByNumber(StageTwoEnemySO);
        SpawnEnemiesByNumber(StageThreeEnemySO);
    }



    private void SpawnEnemiesByNumber(EnemyTypeScriptableObject enemy)
    {
        for(int i = 0; i < enemy.enemyCountBasedOnStage[currentStage - 1]; i++) // scriptable objectben meghatározott mennyiségû enemyt spawnol
        {
            Vector2 spawnPos = GenerateSpawnPosition(SpawnAreaCorner1, SpawnAreaCorner2);
            GameObject newEnemy = Instantiate(enemy.enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.transform.localScale = new Vector2(0.06f, 0.06f);
            newEnemy.transform.rotation = Quaternion.Euler(0, 0, 90);
            newEnemy.GetComponent<EnemyAI>().SetEnemyType(enemy); // beállítja az enemy típust az enemyAI scriptben hogy az tudja milyen statjai vannak
        }
    }
    public Vector2 GenerateSpawnPosition(Transform corner1, Transform corner2)
    {
        Vector2 spawnPos = new Vector2(Random.Range(corner1.position.x, corner2.position.x), Random.Range(corner1.position.y, corner2.position.y));
        return spawnPos;
    }
}

