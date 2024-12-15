using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{   //stage váltásáért felelõs script
    public static StageManager Instance { get; private set; }

    public int currentStage;
    public event EventHandler OnStageInit;

    private void Awake()
    { 
        Instance = this;
        currentStage = SaveManager.Instance.saveData.playerData.currentLevel; //fájlból betöltött adat
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DialogueBoxUI.Instance.isSubscribed == true); //megvárja amíg az event feliratkozik mielõtt meghívja
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }

    public void NextStage() //stage váltásnál hívodik meg
    {
        currentStage++;
        Debug.Log("Current Stage: " + currentStage); 
        //ilyenkor menti el a player adatait
        SaveManager.Instance.SavePlayerData(PlayerHealthManager.Instance.GetPlayerHealth(), ScoreManager.Instance.GetPlayerScore(), currentStage, PlayerMovement.Instance.GetPlayerDamage(),
            PlayerMovement.Instance.GetPlayerShootSpeed(), PlayerMovement.Instance.GetPlayerSpeed(), PlayerMovement.Instance.GetPlayerMaxShield());
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }



}
