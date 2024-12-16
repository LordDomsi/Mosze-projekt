using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    [SerializeField] private Texture2D gameplayCursorTexture;
    [SerializeField] private Texture2D defaultCursorTexture;
    private Vector2 cursorPosition;

    //pause menu megjelenítéséért felelõs script
    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameStateManager.Instance.ToggleGamePause();
        });
        menuButton.onClick.AddListener(() =>
        {
            GameStateManager.Instance.gameState = GameStateManager.GameState.Menu;
            Loader.LoadScene(Loader.Scene.MenuScene);
        });
    }

    private void Start()
    {
        GameStateManager.Instance.OnGamePaused += GameStateManager_OnGamePaused;
        GameStateManager.Instance.OnGameResumed += GameStateManager_OnGameResumed;
        gameObject.SetActive(false);
    }

    private void GameStateManager_OnGameResumed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(false);
        cursorPosition = new Vector2(gameplayCursorTexture.width / 2, gameplayCursorTexture.height / 2);
        Cursor.SetCursor(gameplayCursorTexture, cursorPosition, CursorMode.Auto);
    }

    private void GameStateManager_OnGamePaused(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        cursorPosition = new Vector2(0, 0);
        Cursor.SetCursor(defaultCursorTexture, cursorPosition, CursorMode.Auto);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGamePaused -= GameStateManager_OnGamePaused;
        GameStateManager.Instance.OnGameResumed -= GameStateManager_OnGameResumed;
    }

}
