using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField] private List<GameObject> PowerUpList = new List<GameObject>();     // Power-upok lehetséges prefabjai, 0 a háromlövet, 1 a turbo, 2 WIP
    private Rigidbody2D rigidBody;
    public int PowerUpID = 0;      //melyik power-up

    private const string PLAYER_TAG = "Player";


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            Destroy(this.gameObject);
        }
    }
}
