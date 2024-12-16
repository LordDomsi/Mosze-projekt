using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpIdleAnim : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private void Update() //powerup forog, wow
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
