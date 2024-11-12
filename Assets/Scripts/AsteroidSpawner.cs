using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public static AsteroidSpawner Instance {  get; private set; }

    [SerializeField] private float asteroidSpawnRate; // asteroid�k spawnol�sa k�zti id�
    [SerializeField] private int asteroidMaxNumber; // egyszerre l�tez� maxim�lis asteroid sz�m
     private int asteroidCurrentNumber = 0; // jelenlegi asteroidok sz�ma
    [SerializeField] private List<GameObject> AsteroidList = new List<GameObject>(); //Lehets�ges asteroid prefabok
    [SerializeField] private Transform asteroidSpawnRange1; // a kamer�hoz csatolt gameobjectek amik meghat�rozz�k az asteroid�k spawn hely�t
    [SerializeField] private Transform asteroidSpawnRange2;

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
            Vector2 spawnPosition = GenerateSpawnPosition(asteroidSpawnRange1,asteroidSpawnRange2);

            float size = asteroidMekkora[Random.Range(0, asteroidMekkora.Length)];
            SpawnAsteroid(spawnPosition, size);
            
            time = 0.0f;
        }
    }

    //Aszteroid spawnol�s
    public void SpawnAsteroid(Vector2 spawnPos, float size)
    {
        float var = Random.Range(-15.0f, 15.0f);
        Quaternion rot = Quaternion.AngleAxis(var, Vector3.forward);

        GameObject newAsteroid = Instantiate(AsteroidList[Random.Range(0, AsteroidList.Count)], spawnPos, rot);
        
        newAsteroid.GetComponent<Asteroid>().SetSize(size);
        newAsteroid.transform.localScale = new Vector3(size, size, size);

        asteroidCurrentNumber++;
    }

    public void DecreaseAsteroidCount()
    {
        asteroidCurrentNumber--;
    }

    public Vector2 GenerateSpawnPosition(Transform corner1, Transform corner2)
    {
        Vector2 spawnPos = new Vector2(UnityEngine.Random.Range(corner1.position.x, corner2.position.x), UnityEngine.Random.Range(corner1.position.y, corner2.position.y));
        return spawnPos;
    }


}
