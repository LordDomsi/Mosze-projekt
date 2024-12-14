using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitHealUI : MonoBehaviour
{
    [SerializeField] private Color originalColor;
    [SerializeField] private Color hitColor;
    [SerializeField] private Color healColor;
    [SerializeField] private Image uiImage;
    private float animLength = 0.15f;

    private void Start()
    {
        PlayerHealthManager.Instance.OnPlayerTakeDamage += PlayerHealthManager_OnPlayerTakeDamage;
        PlayerHealthManager.Instance.OnPlayerHeal += PlayerHealthManager_OnPlayerHeal;
        uiImage.color = originalColor;
    }

    private void PlayerHealthManager_OnPlayerHeal(object sender, System.EventArgs e)
    {
        StartCoroutine(Indicator(healColor));
    }

    private void PlayerHealthManager_OnPlayerTakeDamage(object sender, System.EventArgs e)
    {
        StartCoroutine(Indicator(hitColor));
    }

    private IEnumerator Indicator(Color color)
    {
        uiImage.color = color;
        yield return new WaitForSeconds(animLength);
        uiImage.color = originalColor;
    }

    private void OnDestroy()
    {
        PlayerHealthManager.Instance.OnPlayerTakeDamage -= PlayerHealthManager_OnPlayerTakeDamage;
        PlayerHealthManager.Instance.OnPlayerHeal -= PlayerHealthManager_OnPlayerHeal;
    }

}
