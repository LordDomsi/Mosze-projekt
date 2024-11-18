using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    public int currentStage;

    public event EventHandler OnStageInit;

    private void Awake()
    {// ezt az �rt�ket majd f�jlb�l kell bet�lteni 
        Instance = this;
        currentStage = 1;
    }

    private void Start()
    {
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }

    public void NextStage()
    {
        currentStage++;
        Debug.Log("Current Stage: " + currentStage);
        OnStageInit?.Invoke(this, EventArgs.Empty);
    }



}
