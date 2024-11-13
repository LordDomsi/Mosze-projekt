using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enemy adatainak tárolása
[CreateAssetMenu(fileName = "EnemyTypeScriptableObject", menuName = "ScriptableObjects/EnemyTypeScriptableObject")]
public class EnemyTypeScriptableObject : ScriptableObject
{
    public string enemyType;
    public GameObject enemyPrefab;
    public float enemyHealth;
    public float enemySpeed;
    public float enemyShootSpeed;
    public int enemyDamage;
    public float enemyBulletSpeed;
    public List<int> enemyCountBasedOnStage;
}
