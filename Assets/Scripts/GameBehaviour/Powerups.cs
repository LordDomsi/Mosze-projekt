using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string SHIELD_TAG = "Shield";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PLAYER_TAG || collision.gameObject.tag == SHIELD_TAG)
        {
            Destroy(this.gameObject);
        }
    }
}
