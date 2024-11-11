using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Mivel több objektumnak is kell spawn helyet generálni ezért nem árt neki egy statikus osztály ami bárhonnan elérhetõ
public static class SpawnPositionGenerator
{
    public static Vector2 GenerateSpawnPosition(Transform corner1,  Transform corner2)
    {
        Vector2 spawnPos = new Vector2(Random.Range(corner1.position.x, corner2.position.x), Random.Range(corner1.position.y, corner2.position.y));
        return spawnPos;
    }
}
