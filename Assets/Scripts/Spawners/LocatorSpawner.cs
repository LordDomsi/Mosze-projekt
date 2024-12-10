using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocatorSpawner : MonoBehaviour
{
    public static LocatorSpawner Instance {  get; private set; }

    [SerializeField] GameObject locatorPrefab;

    [SerializeField] TextMeshProUGUI locatorText;

    private Animator locatorAnimator;

    public event EventHandler OnLocatorPickup;

    private int currentLocators;



    private void Awake()
    {
        Instance = this;
        // ezt az értéket majd fájlból kell betölteni 
        currentLocators = SaveManager.Instance.saveData.currentLevel-1;
        locatorAnimator = locatorText.gameObject.GetComponent<Animator>();
        UpdateUI();
    }


    //akkor spawnol amikor meghal a boss
    public void Spawn(Transform pos)
    {
        GameObject newObject = Instantiate(locatorPrefab, pos.position, Quaternion.identity);
    }

    public void Pickup()
    {
        currentLocators++;
        locatorAnimator.SetTrigger("Pickup");
        OnLocatorPickup?.Invoke(this, EventArgs.Empty);
        UpdateUI();
    }

    public void UpdateUI()
    {
        locatorText.SetText(currentLocators.ToString() + "|3");
    }

    public int GetCurrentLocators()
    {
        return currentLocators;
    }
    
}
