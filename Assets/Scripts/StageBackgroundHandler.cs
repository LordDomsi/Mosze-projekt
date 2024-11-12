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
    //stage v�lt�sn�l eld�nti melyik map legyen akt�v
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
}
