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
    [SerializeField] private float _timer = 180.0f;
    [SerializeField] private int _maxBuildingDestroyed = 5;
    [SerializeField] private GameObject _orage; //c'est super sale mais faut pas dire
    public int MaxBuildingDestroyed { get => _maxBuildingDestroyed; }

    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    public float CurrentTimer { get; private set; } = 180.0f;

    public int DestroyedBuilding { get; private set; } = 0;

    private void Start()
    {
        Building.OnBuildingDestroyed += OnBuildingDestroyed;
        ChangeGameState(GameState.MainMenu);
    }

    private void OnBuildingDestroyed(Building building)
    {
        DestroyedBuilding++;
        if(DestroyedBuilding >= _maxBuildingDestroyed)
        {
            ChangeGameState(GameState.GameOver);
        }
    }

    public void ChangeGameState(GameState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1.0f;
                CameraManager.Instance.SwitchCamera(toCamGame: false);
                _orage.SetActive(false);
                break;
            case GameState.DebutGame:
                DelayLaunchGame();
                break;
            case GameState.InGame:
                Time.timeScale = 1.0f;
                break;
            case GameState.PauseGame:
                Time.timeScale = 0.0f;
                break;
            case GameState.Victory:
                Victory();
                break;
            case GameState.GameOver:
                GameOver();
                break;
        }
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("Game State: " + CurrentState.ToString());
    }

    private void Update()
    {
        if (CurrentState != GameState.InGame)
            return;

        CurrentTimer -= Time.deltaTime;
        if(CurrentTimer <= 0)
        {
            //End of the game
            ChangeGameState(GameState.Victory);
        }
    }

    private void DelayLaunchGame()
    {
        StartCoroutine(DelayStartCoroutine());
    }

    private IEnumerator DelayStartCoroutine()
    {
        CameraManager.Instance.SwitchCamera(toCamGame: true);
        yield return new WaitForSeconds(2.0f);
        LaunchTimer();
    }
    private void LaunchTimer()
    {
        CurrentTimer = _timer;
        _orage.SetActive(true);
        ChangeGameState(GameState.InGame);
    }

    private void Victory()
    {
        Debug.Log("C'EST LA VICTOIRE OMG WOW");
        Time.timeScale = 0.5f;
    }

    private void GameOver()
    {
        Time.timeScale = 0.5f;
        Debug.Log("T'ES MAUVAIS");
    }

    private void OnDisable()
    {
        Building.OnBuildingDestroyed -= OnBuildingDestroyed;
    }

}
