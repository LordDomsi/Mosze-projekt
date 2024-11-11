using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Mivel t�bb objektumnak is kell spawn helyet gener�lni ez�rt nem �rt neki egy statikus oszt�ly ami b�rhonnan el�rhet�
public static class SpawnPositionGenerator
{
    public static Vector2 GenerateSpawnPosition(Transform corner1,  Transform corner2)
    {
        Vector2 spawnPos = new Vector2(Random.Range(corner1.position.x, corner2.position.x), Random.Range(corner1.position.y, corner2.position.y));
        return spawnPos;
    }
}
