using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
   // private float spawnDelay = 1f;

    private Vector3 offset = new Vector3(12f,0f,0f);

    //csak akkor jelenik meg a fekete lyuk amikor felvesszük a lokátort
    private void Start()
    {
        LocatorSpawner.Instance.OnLocatorPickup += BossSpawner_OnLocatorPickup;
        EnemySpawner.Instance.OnEnemiesSpawned += EnemySpawner_OnEnemiesSpawned;

        this.gameObject.SetActive(false);
    }

    private void EnemySpawner_OnEnemiesSpawned(object sender, System.EventArgs e)
    {
        this.gameObject.SetActive(false);
    }

    private void BossSpawner_OnLocatorPickup(object sender, System.EventArgs e)
    {
        if (StageManager.Instance.currentStage < 3)
        {
            //megjelenés
            this.gameObject.SetActive(true);

            
            //player pozicióhoz képest spawnol
            Vector3 newPos = PlayerMovement.Instance.transform.position;
            newPos.y = 0f;
            newPos += offset;
            this.transform.position = newPos;

            PopupManager.Instance.StartBlackHoleAnim(this.gameObject); // feketelyuk megjelenésénél lévõ animáció
        }
    }

    private void OnDestroy()
    {
        LocatorSpawner.Instance.OnLocatorPickup -= BossSpawner_OnLocatorPickup;
        EnemySpawner.Instance.OnEnemiesSpawned -= EnemySpawner_OnEnemiesSpawned;
    }

}
