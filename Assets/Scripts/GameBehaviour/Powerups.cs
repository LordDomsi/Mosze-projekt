using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField] private List<GameObject> PowerUpList = new List<GameObject>();     // Power-upok lehetséges prefabjai, 0 a háromlövet, 1 a speed, 2 shield
    public int PowerUpID = 0;      //melyik power-up

    private const string PLAYER_TAG = "Player";

    public void PowerUpSpawn(Vector2 spawnPos)
    {
        PowerUpID = Random.Range(0, 3);     //a power-up random

        Quaternion rotate = Quaternion.AngleAxis(0, Vector3.forward);
        GameObject newPowerUp = Instantiate(PowerUpList[PowerUpID], spawnPos, rotate);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG)
        {
            Destroy(this.gameObject);
        }
    }
}
