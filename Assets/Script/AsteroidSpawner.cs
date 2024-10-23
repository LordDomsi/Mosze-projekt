using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid aszteroida;

    [SerializeField] private float AsteroidSpawnRate = 10.0f;

    private float[] asteroidMekkora = new float[3] { 0.5f, 1.0f, 2.0f };        //kicsi, kozepes, nagy

    void Start()
    {
        InvokeRepeating(nameof(spawn), this.AsteroidSpawnRate, this.AsteroidSpawnRate);
    }

    private void spawn()           //bug: amikor a spawn megtortenik, lemasolja a jelenleg elo asteroidakat is, javitva!!!
    {
        Vector3 SpawnPos = new Vector3(Random.Range(-15.0f, 15.0f), Random.Range(-15.0f, 15.0f), 1);

        float var = Random.Range(-15.0f, 15.0f);
        Quaternion rot = Quaternion.AngleAxis(var, Vector3.forward);

        Asteroid asteroid = Instantiate(aszteroida, SpawnPos, rot);
        asteroid.size = Random.Range(0, asteroidMekkora.Length);
        asteroid.Irany(rot * -SpawnPos, asteroid.size);
    }

    
}
