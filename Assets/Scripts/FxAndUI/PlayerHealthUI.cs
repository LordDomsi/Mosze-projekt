using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    //healthbar visual script
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image Outline;
    [SerializeField] private Image Glow;
    [SerializeField] private TextMeshProUGUI HealthText;

    [SerializeField] private Color hitColor;
    [SerializeField] private Color healColor;
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
        PlayerHealthManager.Instance.OnPlayerHeal += PlayerHealthManager_OnPlayerHeal;
        UpdateHealthBar();
    }

    private void PlayerHealthManager_OnPlayerTakeDamage(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
        StartCoroutine(HitAnim());
    }

    private void PlayerHealthManager_OnPlayerHeal(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
        StartCoroutine(HealAnim());
    }

    public void UpdateHealthBar() //healthbar frissítése
    {
        float fillAm = (float)PlayerHealthManager.Instance.GetPlayerHealth() / (float)PlayerHealthManager.Instance.GetPlayerMaxHealth();
        HealthBar.fillAmount = fillAm;
        HealthText.text = PlayerHealthManager.Instance.GetPlayerHealth().ToString();
    }
    private IEnumerator HitAnim() //animáció amikor a player sebzõdik
    {
        HealthBar.color = hitColor;
        Outline.color = hitColor;
        Glow.color = hitColor;
        HealthText.color = hitColor;
        yield return new WaitForSeconds(animLength);
        HealthBar.color = originalColorBar;
        Outline.color = originalColorOutline;
        Glow.color = originalColorGlow;
        HealthText.color = Color.white;
    }

    private IEnumerator HealAnim() //animáció amikor a player healt kap
    {
        HealthBar.color = healColor;
        Outline.color = healColor;
        Glow.color = healColor;
        HealthText.color = healColor;
        yield return new WaitForSeconds(animLength);
        HealthBar.color = originalColorBar;
        Outline.color = originalColorOutline;
        Glow.color = originalColorGlow;
        HealthText.color = Color.white;
    }

    private void OnDestroy()
    {
        PlayerHealthManager.Instance.OnPlayerTakeDamage -= PlayerHealthManager_OnPlayerTakeDamage;
        PlayerHealthManager.Instance.OnPlayerHeal -= PlayerHealthManager_OnPlayerHeal;
    }
}
