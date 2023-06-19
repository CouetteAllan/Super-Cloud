using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.DebutGame:
                break;
            case GameState.InGame:
                //Hide pause panel
                break;
            case GameState.PauseGame:
                //Display pause panel
                break;
            case GameState.Victory:
                break;
            case GameState.GameOver:
                //Display Game Over panel
                break;
        }
    }
}
