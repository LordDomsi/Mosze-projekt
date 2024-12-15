using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    //pontszám megjelenítése
    [SerializeField] private TextMeshProUGUI scoreText;
    private Color originalColor;
    [SerializeField] private Color increaseColor;
    private float animLength = 0.15f;
    private float defaultFontSize = 50f;
    private float increaseFontSize = 65f;

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
        UpdateScoreUI(e.Score); // score változásakor frissíti a ui-t
        StartCoroutine(HitAnim());
    }

    public void UpdateScoreUI(float score)
    {
        scoreText.SetText(score.ToString());
    }

    private IEnumerator HitAnim() //animáció amikor nõ a score
    {
        scoreText.color = increaseColor;
        scoreText.fontSize = increaseFontSize;
        yield return new WaitForSeconds(animLength);
        scoreText.color = originalColor;
        scoreText.fontSize = defaultFontSize;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnScoreIncrease -= ScoreManager_OnScoreIncrease;
    }
}
