using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    //a men� gombok�rt felel�s script
    public static MainMenuUI Instance {  get; private set; }

    [SerializeField] private Texture2D defaultCursor;
    private Vector2 cursorPosition;

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button settingApplyButton;
    [SerializeField] private Button leaderboardCloseButton;

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject leaderboardUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject titleUI;

    [SerializeField] private TMP_InputField nameInputField;

    [SerializeField] private AnimationCurve UIElementCurve;
    [SerializeField] private float uiElementSpeed;

    private const string SFXExposedParam = "SFXVolumeExposedParam";
    private const string MusicExposedParam = "MusicVolumeExposedParam";

    private void Awake()
    {
        Instance = this;

        newGameButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.SavePlayerData(100, 0, 1, 1f, 0.5f, 250f, 3); //new game-n�l reseteli az elmentett player dat�t 
            GameStateManager.Instance.gameState = GameStateManager.GameState.NewGame;
            Loader.LoadScene(Loader.Scene.CutScene);
        });
        continueButton.onClick.AddListener(() =>
        {
            GameStateManager.Instance.gameState = GameStateManager.GameState.Game;
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
        nameInputField.onValueChanged.AddListener((string newText) =>
        {
            SaveManager.Instance.SaveEnteredName(newText); //elmenti a be�rt nevet
        });
        sfxSlider.onValueChanged.AddListener((float volume) =>
        {
            SaveManager.Instance.SaveSFXData(volume); //a slider �rt�k�nek v�ltoz�sakor elmenti az �j �rt�ket
            AudioManager.Instance.SetVolume(volume, SFXExposedParam, sfxSlider); //ezut�n a mixert is updateli
        });
        musicSlider.onValueChanged.AddListener((float volume) =>
        {
            SaveManager.Instance.SaveMusicData(volume);
            AudioManager.Instance.SetVolume(volume, MusicExposedParam, musicSlider);
        });
    }
    
    private IEnumerator Start()
    {
        cursorPosition = new Vector2(0,0);
        Cursor.SetCursor(defaultCursor, cursorPosition, CursorMode.Auto);
        Time.timeScale = 1.0f;
        AudioManager.Instance.PlayMusic(AudioManager.Music_enum.MENU_THEME);
        StartCoroutine(PopupManager.Instance.PopupCurveAnim(titleUI, uiElementSpeed, UIElementCurve));
        StartCoroutine(PopupManager.Instance.PopupCurveAnim(menuUI, uiElementSpeed, UIElementCurve));
        yield return new WaitUntil(() => SaveManager.Instance.loaded == true);
        nameInputField.text = SaveManager.Instance.saveData.enteredName;//az inputfield textet updateli miut�n bet�lt�tt a f�jl 
        sfxSlider.value = SaveManager.Instance.saveData.settings.sfxVolume; //a slidereket is updateli ind�t�skor
        musicSlider.value = SaveManager.Instance.saveData.settings.musicVolume;
    }

}
