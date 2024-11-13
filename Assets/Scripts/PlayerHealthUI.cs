using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image HealthBar;

    private void Start()
    {
        PlayerHealthManager.Instance.OnPlayerTakeDamage += PlayerHealthManager_OnPlayerTakeDamage;
        UpdateHealthBar();
    }

    private void PlayerHealthManager_OnPlayerTakeDamage(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float fillAm = (float)PlayerHealthManager.Instance.GetPlayerHealth() / (float)PlayerHealthManager.Instance.GetPlayerMaxHealth();
        HealthBar.fillAmount = fillAm;
    }
}
