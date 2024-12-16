using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBackgroundHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> stageTilemaps = new List<GameObject>();

    private void Start()
    {
        StageManager.Instance.OnStageInit += Stagemanager_OnStageInit;
    }
    //stage váltásnál eldönti melyik map legyen aktív
    //pályaváltásnál lényegében csak a háttér változik meg amúgy ugyanoda spawnol a player mindig
    private void Stagemanager_OnStageInit(object sender, System.EventArgs e)
    {
        for(int i = 0; i< stageTilemaps.Count; i++)
        {
            if(StageManager.Instance.currentStage == i + 1)
            {
                stageTilemaps[i].SetActive(true);
            }
            else
            {
                stageTilemaps[i].SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        StageManager.Instance.OnStageInit -= Stagemanager_OnStageInit;
    }
}
