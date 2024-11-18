using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopupDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    public int score = 0;
    private float fadeDuration = 5f;

    private void Start()
    {
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
        scoreText.SetText("+" + score.ToString());
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        Destroy(this.gameObject);
    }
}
