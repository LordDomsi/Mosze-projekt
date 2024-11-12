using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField] private List<GameObject> PowerUpList = new List<GameObject>();     // Power-upok lehetséges prefabjai
    private Rigidbody2D rigidBody;
    private int PowerUpID = 0;      //melyik power-up


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void PowerUpSpawn(Vector2 spawnPos)
    {
        PowerUpID = Random.Range(0, 3);     //a power-up random

        Quaternion rotate = Quaternion.AngleAxis(0, Vector3.forward);
        GameObject newPowerUp = Instantiate(PowerUpList[PowerUpID], spawnPos, rotate);

    }

    private void Update()
    {
        
    }
}
