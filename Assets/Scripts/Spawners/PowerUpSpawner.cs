using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public static PowerUpSpawner Instance {  get; private set; }

    [SerializeField] private List<GameObject> PowerUpList = new List<GameObject>();     // Power-upok lehetséges prefabjai, 0 a háromlövet, 1 a speed, 2 shield, 3 health
    private int PowerUpID;      //melyik power-up


    private void Awake()
    {
        Instance = this;
    }

    public void PowerUpSpawn(Vector2 spawnPos)
    {

        PowerUpID = GeneratePowerUpID();
        Quaternion rotate = Quaternion.AngleAxis(0, Vector3.forward);
        GameObject newPowerUp = Instantiate(PowerUpList[PowerUpID], spawnPos, rotate);
    }

    public int GeneratePowerUpID()
    {
        int powerUpId = 0;
        int number = Random.Range(0, 101);
        if (number < 41) //40% esély health powerup, a többi 20%
        {
            powerUpId = 3;
        }
        else if (number < 61)
        {
            powerUpId = 2;
        }
        else if (number < 81)
        {
            powerUpId = 1;
        }
        else
        {
            powerUpId = 0;
        }
        return powerUpId;
    }
}
