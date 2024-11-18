using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlackHole : MonoBehaviour
{
    private Vector3 offset = new Vector3(10f,0f,0f);

    //csak akkor jelenik meg a fekete lyuk amikor legy�zt�k a jelenlegi stage bosst
    private void Start()
    {
        BossSpawner.Instance.OnBossDeath += BossSpawner_OnBossDeath;
        EnemySpawner.Instance.OnEnemiesSpawned += EnemySpawner_OnEnemiesSpawned;

        this.gameObject.SetActive(false);
    }

    private void EnemySpawner_OnEnemiesSpawned(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(false);
    }

    private void BossSpawner_OnBossDeath(object sender, System.EventArgs e)
    {
       if (StageManager.Instance.currentStage < 3)
        {
            //megjelen�s
            this.gameObject.SetActive(true);

            //player pozici�hoz k�pest spawnol
            Vector3 newPos = PlayerMovement.Instance.transform.position;
            newPos.y = 0f;
            newPos += offset;
            this.transform.position = newPos;

            PopupManager.Instance.StartBlackHoleAnim(this.gameObject); // feketelyuk megjelen�s�n�l l�v� anim�ci�
        }
        
    }
    
}
