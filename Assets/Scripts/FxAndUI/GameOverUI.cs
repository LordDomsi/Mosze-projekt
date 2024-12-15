using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    [SerializeField] private Button menuButton;
    public bool gameOver = false;

    [SerializeField] private Texture2D defaultCursorTexture;
    private Vector2 cursorPosition;
   
    private void Awake()
    {
        Instance = this;
        menuButton.onClick.AddListener(() =>
        {
            GameStateManager.Instance.gameState = GameStateManager.GameState.Menu;
            Loader.LoadScene(Loader.Scene.MenuScene);
        });
    }

    private void Start()
    {
        PlayerHealthManager.Instance.OnPlayerDeath += PlayerHealtManager_OnPlayerDeath;
        gameObject.SetActive(false);
    }
    //game over ui megjelenítése
    private void PlayerHealtManager_OnPlayerDeath(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        cursorPosition = new Vector2(0, 0);
        Cursor.SetCursor(defaultCursorTexture, cursorPosition, CursorMode.Auto);
        gameOver = true;
        AudioManager.Instance.PlayMusic(AudioManager.Music_enum.GAMEOVER_THEME);
    }

    private void OnDestroy()
    {
        PlayerHealthManager.Instance.OnPlayerDeath -= PlayerHealtManager_OnPlayerDeath;
    }
}
