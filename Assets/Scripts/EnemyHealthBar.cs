using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image HealthBar;
    [SerializeField] private GameObject HealthBarParent;

    private void Update()
    {
        HealthBarParent.transform.rotation = Camera.main.transform.rotation;
    }
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (currentValue == maxValue) HealthBarParent.gameObject.SetActive(false);
        else HealthBarParent.gameObject.SetActive(true);
        HealthBar.fillAmount = currentValue/maxValue;
    }

}
