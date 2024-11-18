using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private Color originalColor;
    [SerializeField] private Color increaseColor;
    private float animLength = 0.15f;

    private void Awake()
    {
        originalColor = scoreText.color;
    }
    private void Start()
    {
        ScoreManager.Instance.OnScoreIncrease += ScoreManager_OnScoreIncrease;
        UpdateScoreUI(ScoreManager.Instance.GetPlayerScore());
    }

    private void ScoreManager_OnScoreIncrease(object sender, ScoreManager.OnScoreIncreaseEventArgs e)
    {
        UpdateScoreUI(e.Score); // score v�ltoz�sakor friss�ti a ui-t
        StartCoroutine(HitAnim());
    }

    public void UpdateScoreUI(float score)
    {
        scoreText.SetText(score.ToString());
    }

    private IEnumerator HitAnim()
    {
        scoreText.color = increaseColor;
        yield return new WaitForSeconds(animLength);
        scoreText.color = originalColor;
    }
}
