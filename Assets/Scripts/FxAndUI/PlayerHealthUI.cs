using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image Outline;
    [SerializeField] private Image Glow;

    [SerializeField] private Color hitColor;
    private Color originalColorBar;
    private Color originalColorOutline;
    private Color originalColorGlow;

    private float animLength = 0.15f;
    private void Awake()
    {
        originalColorBar = HealthBar.color;
        originalColorOutline = Outline.color;
        originalColorGlow = Glow.color;
    }

    private void Start()
    {
        PlayerHealthManager.Instance.OnPlayerTakeDamage += PlayerHealthManager_OnPlayerTakeDamage;
        UpdateHealthBar();
    }

    private void PlayerHealthManager_OnPlayerTakeDamage(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
        StartCoroutine(HitAnim());
    }

    public void UpdateHealthBar()
    {
        float fillAm = (float)PlayerHealthManager.Instance.GetPlayerHealth() / (float)PlayerHealthManager.Instance.GetPlayerMaxHealth();
        HealthBar.fillAmount = fillAm;
    }
    private IEnumerator HitAnim()
    {
        HealthBar.color = hitColor;
        Outline.color = hitColor;
        Glow.color = hitColor;
        yield return new WaitForSeconds(animLength);
        HealthBar.color = originalColorBar;
        Outline.color = originalColorOutline;
        Glow.color = originalColorGlow;
    }
}
