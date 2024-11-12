using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleDetect : MonoBehaviour
{
    //ha hozzáér a player meghívja a pályaváltást
    [SerializeField] private Transform SpawnPosition;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PopupManager.Instance.Transition(other.gameObject, SpawnPosition);
        }
    }
}
