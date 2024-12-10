using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    //a menü gombokért felelõs script
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

    public event EventHandler OnSettingsClicked;

    private const string SFXExposedParam = "SFXVolumeExposedParam";
    private const string MusicExposedParam = "MusicVolumeExposedParam";

    private void Awake()
    {
        Instance = this;

        newGameButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveLevel(1);
            SaveManager.Instance.SaveScore(0);
            SaveManager.Instance.SaveHealth(100);
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
            Debug.Log("new name saved: "+ newText);
            SaveManager.Instance.SaveEnteredName(newText); //elmenti a beírt nevet
        });
        sfxSlider.onValueChanged.AddListener((float volume) =>
        {
            Debug.Log("sfx volume updated " + volume);
            SaveManager.Instance.SaveSFXData(volume); //a slider értékének változásakor elmenti az új értéket
            AudioManager.Instance.SetVolume(volume, SFXExposedParam, sfxSlider); //ezután a mixert is updateli
        });
        musicSlider.onValueChanged.AddListener((float volume) =>
        {
            Debug.Log("music volume updated " + volume);
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
        nameInputField.text = SaveManager.Instance.saveData.enteredName;//az inputfield textet updateli miután betöltött a fájl 
        sfxSlider.value = SaveManager.Instance.saveData.settings.sfxVolume; //a slidereket is updateli indításkor
        musicSlider.value = SaveManager.Instance.saveData.settings.musicVolume;
    }

}
