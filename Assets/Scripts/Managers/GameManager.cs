using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    MainMenu,
    DebutGame, //Si jamais on fait une intro, on passe d'abord par DebutGame puis on va sur InGame
    InGame,
    PauseGame,
    Victory,
    GameOver
}

public class GameManager : Singleton<GameManager>
{
    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    public void ChangeGameState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                break;
            case GameState.DebutGame:
                break;
            case GameState.InGame:
                break;
            case GameState.PauseGame:
                break;
            case GameState.Victory:
                break;
            case GameState.GameOver:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }
}
