using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int currentStage;

    public event EventHandler OnStageInit;

    private void Awake()
    {// ezt az értéket majd fájlból kell betölteni 
        Instance = this;
        currentStage = SaveManager.Instance.saveData.currentLevel;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DialogueBoxUI.Instance.isSubscribed == true);
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }

    public void NextStage()
    {
        currentStage++;
        Debug.Log("Current Stage: " + currentStage);
        SaveManager.Instance.SaveLevel(currentStage);
        SaveManager.Instance.SaveScore(ScoreManager.Instance.GetPlayerScore());
        SaveManager.Instance.SaveHealth(PlayerHealthManager.Instance.GetPlayerHealth());
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }



}
