using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BlackHole : MonoBehaviour
{
    

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
       if (StageManager.Instance.currentStage < 3)
        {
           // this.gameObject.SetActive(true);
            //PopupManager.Instance.StartBlackHoleAnim(this.gameObject); // feketelyuk megjelen�s�n�l l�v� anim�ci�
        }
        
    }


    //ha a player hozz��r bet�lti a k�vetkez� p�ly�t
    
}
