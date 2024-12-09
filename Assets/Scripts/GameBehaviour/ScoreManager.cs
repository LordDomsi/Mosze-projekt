using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //player pontsz�m�t�s��rt felel�s script
    public static ScoreManager Instance { get; private set; }

    private int playerScore;

    public event EventHandler<OnScoreIncreaseEventArgs> OnScoreIncrease;
    public class OnScoreIncreaseEventArgs
    {
        public int Score;
    }

    private void Awake()
    {
        Instance = this;
        playerScore = SaveManager.Instance.saveData.currentScore;
    }

    

    public void IncreasePlayerScore(int playerScore)
    {
        this.playerScore += playerScore;
        OnScoreIncrease?.Invoke(this, new OnScoreIncreaseEventArgs { Score = this.playerScore}); // �tadjuk az eventnek a  scoret hogy friss�tse a ui-t
    }

    public int GetPlayerScore()
    {
        return this.playerScore;
    }

}
