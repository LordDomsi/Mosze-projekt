using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float timeBeforeDestroy = 0.7f;
    private void Start()
    {
        StartCoroutine(WaitThenDestroySelf());
    }

    private IEnumerator WaitThenDestroySelf()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
}
