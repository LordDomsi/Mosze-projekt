using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance {  get; private set; }

    //game statek menedzsel�se
    public enum GameState
    {
        Menu,
        NewGame,
        Game,
        Ending
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameResumed;

    private bool isGamePaused = false;

    public bool allowedToPause = true;

    public GameState gameState;
    private void Awake()
    {
        if (Instance != null)   
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        gameState = GameState.Menu; //j�t�k kezd�sn�l menu
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameState == GameState.Game && allowedToPause) //j�t�k meg�ll�t�sa
        {
            if(!GameOverUI.Instance.gameOver) ToggleGamePause();
        }
    }

    public void ToggleGamePause()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused) { StopGame(); OnGamePaused?.Invoke(this, EventArgs.Empty); }
        else { ContinueGame(); OnGameResumed?.Invoke(this, EventArgs.Empty); }
    }

    public void ContinueGame()
    {
        if(!DialogueBoxUI.Instance.displayingText)Time.timeScale = 1f;
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
    }

    public bool IsGamePaused()
    {
        return isGamePaused;
    }
}
