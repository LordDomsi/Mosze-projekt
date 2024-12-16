using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUI : MonoBehaviour
{
    public static ShieldUI Instance { get; private set; }

    [SerializeField] private Transform container;
    [SerializeField] private Transform shieldUITemplate;

    [SerializeField] private Shield playerShield;

    private void Awake()
    {
        Instance = this;
        shieldUITemplate.gameObject.SetActive(false);
    }

    public void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == shieldUITemplate) continue;
            Destroy(child.gameObject);
        }

        for(int i = 0; i < playerShield.GetShieldHealth(); i++)
        {
            Transform newUIElement = Instantiate(shieldUITemplate, container);
            newUIElement.gameObject.SetActive(true);
        }
    }
}
