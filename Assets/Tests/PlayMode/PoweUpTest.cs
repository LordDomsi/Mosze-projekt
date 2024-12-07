using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField] private List<GameObject> PowerUpList = new List<GameObject>(); 
    private Rigidbody2D rigidBody;
    public int PowerUpID = 0;

    private const string PLAYER_TAG = "Player";

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void PowerUpSpawn(Vector2 spawnPos)
    {
        PowerUpID = Random.Range(0, PowerUpList.Count); 
        Quaternion rotate = Quaternion.identity;
        GameObject newPowerUp = Instantiate(PowerUpList[PowerUpID], spawnPos, rotate);

        newPowerUp.transform.position = spawnPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            Destroy(gameObject);
        }
    }
}
