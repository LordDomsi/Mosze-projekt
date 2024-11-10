using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public static AsteroidSpawner Instance {  get; private set; }

    [SerializeField] private float asteroidSpawnRate; // asteroidák spawnolása közti idõ
    [SerializeField] private int asteroidMaxNumber; // egyszerre létezõ maximális asteroid szám
     private int asteroidCurrentNumber = 0; // jelenlegi asteroidok száma
    [SerializeField] private List<GameObject> AsteroidList = new List<GameObject>(); //Lehetséges asteroid prefabok
    [SerializeField] private GameObject asteroidSpawnRange1; // a kamerához csatolt gameobjectek amik meghatározzák az asteroidák spawn helyét
    [SerializeField] private GameObject asteroidSpawnRange2;

    private float[] asteroidMekkora = new float[3] { 0.04f, 0.08f, 0.16f };        //kicsi, kozepes, nagy

    private float time = 0.0f;

    private void Awake()
    {
        Instance = this;
    }

    //Timer
    private void Update()
    {
        time += Time.deltaTime;
        if(time > asteroidSpawnRate && asteroidCurrentNumber<asteroidMaxNumber)
        {
            Vector2 spawnPosition = GenerateSpawnPosition();

            float size = asteroidMekkora[Random.Range(0, asteroidMekkora.Length)];
            SpawnAsteroid(spawnPosition, size);
            
            time = 0.0f;
        }
    }

    //Aszteroid spawnolás
    public void SpawnAsteroid(Vector2 spawnPos, float size)
    {
        float var = Random.Range(-15.0f, 15.0f);
        Quaternion rot = Quaternion.AngleAxis(var, Vector3.forward);

        GameObject newAsteroid = Instantiate(AsteroidList[Random.Range(0, AsteroidList.Count)], spawnPos, rot);
        
        newAsteroid.GetComponent<Asteroid>().SetSize(size);
        newAsteroid.transform.localScale = new Vector3(size, size, size);

        asteroidCurrentNumber++;
    }


    //spawn pozicíó generálása
    private Vector2 GenerateSpawnPosition()
    {
        Vector2 spawnPos = new Vector2(Random.Range(asteroidSpawnRange1.transform.position.x, asteroidSpawnRange2.transform.position.x), Random.Range(asteroidSpawnRange1.transform.position.y, asteroidSpawnRange2.transform.position.y));
        return spawnPos;
    }

    public void DecreaseAsteroidCount()
    {
        asteroidCurrentNumber--;
    }

    
}
