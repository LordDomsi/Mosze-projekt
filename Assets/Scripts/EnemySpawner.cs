using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //az enemyk adatait t�rol� sciptable objetek
    [SerializeField] private EnemyTypeScriptableObject StageOneEnemySO;
    [SerializeField] private EnemyTypeScriptableObject StageTwoEnemySO;
    [SerializeField] private EnemyTypeScriptableObject StageThreeEnemySO;

    //K�t objektum �ltal meghat�rozzot helyen spawnolnak az enemyk
    [SerializeField] private Transform SpawnAreaCorner1;
    [SerializeField] private Transform SpawnAreaCorner2;

    private int currentStage = 1;

    //Ez nem starton lesz hanem amikor staget v�ltunk csak az m�g nincs meg�rva
    private void Start()
    {
        SpawnEnemiesByNumber(StageOneEnemySO);
        SpawnEnemiesByNumber(StageTwoEnemySO);
        SpawnEnemiesByNumber(StageThreeEnemySO);
    }

    private void SpawnEnemiesByNumber(EnemyTypeScriptableObject enemy)
    {
        for(int i = 0; i < enemy.enemyCountBasedOnStage[currentStage - 1]; i++)
        {
            Vector2 spawnPos = SpawnPositionGenerator.GenerateSpawnPosition(SpawnAreaCorner1, SpawnAreaCorner2);
            GameObject newEnemy = Instantiate(enemy.enemyPrefab, spawnPos, Quaternion.identity);
            newEnemy.name = enemy.enemyType;
            newEnemy.transform.localScale = new Vector2(0.06f, 0.06f);
        }
    }
}
