using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance {  get; private set; }

    public enum GameState
    {
        Menu,
        NewGame,
        Game,
        Ending
    }

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

        gameState = GameState.Menu;
        DontDestroyOnLoad(this.gameObject);
    }
}
