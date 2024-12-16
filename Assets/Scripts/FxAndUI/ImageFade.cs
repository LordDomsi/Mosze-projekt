using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    private float fadeTime = 1f;
    private Color originalColor;
    [SerializeField]private Image fadeImage;

    private void Awake()
    {
        fadeImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        DialogueBoxUI.Instance.OnGameEnd += DialogueBoxUI_OnGameEnd;
    }

    private void DialogueBoxUI_OnGameEnd(object sender, System.EventArgs e)
    {
        StartCoroutine(FadeIn());
    }

    //képernyõ fade amikor vége a játéknak
    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(fadeTime);
        fadeImage.gameObject.SetActive (true);
        originalColor = fadeImage.color;
        fadeImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        float time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f,1f,time/fadeTime);
            fadeImage.color = new Color(originalColor.r,originalColor.g,originalColor.b,alpha);
            yield return null;
        }

        fadeImage.color = new Color(originalColor.r, originalColor.g,originalColor.b,1f);
    }

    private void OnDestroy()
    {
        DialogueBoxUI.Instance.OnGameEnd -= DialogueBoxUI_OnGameEnd;
    }
}
