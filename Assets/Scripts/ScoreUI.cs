using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        ScoreManager.Instance.OnScoreIncrease += ScoreManager_OnScoreIncrease;
        UpdateScoreUI(ScoreManager.Instance.GetPlayerScore());
    }

    private void ScoreManager_OnScoreIncrease(object sender, ScoreManager.OnScoreIncreaseEventArgs e)
    {
        UpdateScoreUI(e.Score); // score változásakor frissíti a ui-t
    }

    public void UpdateScoreUI(float score)
    {
        scoreText.SetText(score.ToString());
    }
}
