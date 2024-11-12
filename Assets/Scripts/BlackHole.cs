using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlackHole : MonoBehaviour
{
    [SerializeField]private Transform SpawnPosition;

    //csak akkor jelenik meg a fekete lyuk amikor legy�zt�k az �sszes  ellenfelet
    private void Start()
    {
        EnemySpawner.Instance.OnEnemiesCleared += EnemySpawner_OnEnemiesCleared;
        EnemySpawner.Instance.OnEnemiesSpawned += EnemySpawner_OnEnemiesSpawned;

        this.gameObject.SetActive(false);
    }

    private void EnemySpawner_OnEnemiesSpawned(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(false);
    }

    private void EnemySpawner_OnEnemiesCleared(object sender, System.EventArgs e)
    {
        if(StageManager.Instance.currentStage<3) this.gameObject.SetActive(true);
    }


    //ha a player hozz��r bet�lti a k�vetkez� p�ly�t
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = SpawnPosition.position;// visszarakja a playert a kezd�helyzetbe
            if (StageManager.Instance.currentStage < 3) StageManager.Instance.NextStage();
        }
    }
}
