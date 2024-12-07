using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    //a menü gombokért felelõs script
    public static MainMenuUI Instance {  get; private set; }

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button settingApplyButton;
    [SerializeField] private Button leaderboardCloseButton;

    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject leaderboardUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject titleUI;

    [SerializeField] private AnimationCurve UIElementCurve;
    [SerializeField] private float uiElementSpeed;

    public event EventHandler OnSettingsClicked;

    private void Awake()
    {
        Instance = this;

        newGameButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.GameScene);
        });
        continueButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.GameScene);
        });
        leaderboardButton.onClick.AddListener(() =>
        {
            leaderboardUI.SetActive(true);
            StartCoroutine(PopupManager.Instance.PopupCurveAnim(leaderboardUI, uiElementSpeed, UIElementCurve));
            menuUI.SetActive(false);
        });
        settingsButton.onClick.AddListener(() =>
        {
            settingsUI.SetActive(true);
            StartCoroutine(PopupManager.Instance.PopupCurveAnim(settingsUI, uiElementSpeed, UIElementCurve));
            menuUI.SetActive(false);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        settingApplyButton.onClick.AddListener(() =>
        {
            menuUI.SetActive(true);
            StartCoroutine(PopupManager.Instance.PopupCurveAnim(menuUI, uiElementSpeed, UIElementCurve));
            settingsUI.SetActive(false);
        });
        leaderboardCloseButton.onClick.AddListener(() =>
        {
            menuUI.SetActive(true);
            StartCoroutine(PopupManager.Instance.PopupCurveAnim(menuUI, uiElementSpeed, UIElementCurve));
            leaderboardUI.SetActive(false);
        });
    }

    private void Start()
    {
        StartCoroutine(PopupManager.Instance.PopupCurveAnim(titleUI, uiElementSpeed, UIElementCurve));
        StartCoroutine(PopupManager.Instance.PopupCurveAnim(menuUI, uiElementSpeed, UIElementCurve));
    }
}
