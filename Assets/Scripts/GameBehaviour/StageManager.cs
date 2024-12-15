using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{   //stage v�lt�s��rt felel�s script
    public static StageManager Instance { get; private set; }

    public int currentStage;
    public event EventHandler OnStageInit;

    private void Awake()
    { 
        Instance = this;
        currentStage = SaveManager.Instance.saveData.playerData.currentLevel; //f�jlb�l bet�lt�tt adat
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DialogueBoxUI.Instance.isSubscribed == true); //megv�rja am�g az event feliratkozik miel�tt megh�vja
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }

    public void NextStage() //stage v�lt�sn�l h�vodik meg
    {
        currentStage++;
        Debug.Log("Current Stage: " + currentStage); 
        //ilyenkor menti el a player adatait
        SaveManager.Instance.SavePlayerData(PlayerHealthManager.Instance.GetPlayerHealth(), ScoreManager.Instance.GetPlayerScore(), currentStage, PlayerMovement.Instance.GetPlayerDamage(),
            PlayerMovement.Instance.GetPlayerShootSpeed(), PlayerMovement.Instance.GetPlayerSpeed(), PlayerMovement.Instance.GetPlayerMaxShield());
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }



}
