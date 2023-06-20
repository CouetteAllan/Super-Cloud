using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _timerText;

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

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.InGame)
            return;

        float currentTime = GameManager.Instance.CurrentTimer;
        // on choppes le nombre de minutes dans current time
        int m = Mathf.FloorToInt(currentTime / 60F);

        // le nombre de secondes dans la minute convertie
        int s = Mathf.FloorToInt(currentTime - (m * 60));

        // magie noire ogm
        // en gros juste on affiche "minutes : secondes"
        // et si les secondes sont < 10 on ajoute un 0 devant 
        string joliTempsPouceEnLair = $"{m}:{(s > 10 ? s : ("0" + s))}";
        _timerText.text = joliTempsPouceEnLair;
    }
}
