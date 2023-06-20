using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private GameObject _buildingPanel;
    [SerializeField] private GameObject _pauseGO;
    [SerializeField] private GameObject _mainMenuGO;
    [SerializeField] private GameObject _victoryGO;
    [SerializeField] private GameObject _gameOverGO;

    private enum PanelType
    {
        MainMenu,
        Pause,
        Victory,
        GameOver,
        None
    }

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                DisplayPanel(PanelType.MainMenu);
                _buildingPanel.SetActive(false);

                break;
            case GameState.DebutGame:
                _mainMenuGO.SetActive(false);


                break;
            case GameState.InGame:
                DisplayPanel(PanelType.None);

                //Hide pause panel
                break;
            case GameState.PauseGame:
                DisplayPanel(PanelType.Pause);

                //Display pause panel
                break;
            case GameState.Victory:
                DisplayPanel(PanelType.Victory);

                break;
            case GameState.GameOver:
                DisplayPanel(PanelType.GameOver);
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
        string joliTempsPouceEnLair = $"{m}:{(s >= 10 ? s : ("0" + s))}";
        _timerText.text = joliTempsPouceEnLair;
    }

    private void DisplayPanel(PanelType panelType)
    {
        switch (panelType)
        {
            case PanelType.MainMenu:
                _mainMenuGO.SetActive(true);
                _pauseGO.SetActive(false);
                _gameOverGO.SetActive(false);
                _victoryGO.SetActive(false);
                _timerPanel.SetActive(false);
                _buildingPanel.SetActive(false);
                break;
            case PanelType.Pause:
                _pauseGO.SetActive(true);
                _mainMenuGO.SetActive(false);
                _gameOverGO.SetActive(false);
                _victoryGO.SetActive(false);
                _timerPanel.SetActive(false);
                break;
            case PanelType.Victory:
                _victoryGO.SetActive(true);
                _mainMenuGO.SetActive(false);
                _pauseGO.SetActive(false);
                _gameOverGO.SetActive(false);
                _timerPanel.SetActive(false);
                break;
            case PanelType.GameOver:
                _gameOverGO.SetActive(true);
                _mainMenuGO.SetActive(false);
                _pauseGO.SetActive(false);
                _victoryGO.SetActive(false);

                break;
            case PanelType.None:
                _gameOverGO.SetActive(false);
                _mainMenuGO.SetActive(false);
                _pauseGO.SetActive(false);
                _victoryGO.SetActive(false);
                _timerPanel.SetActive(true);
                _buildingPanel.SetActive(true);

                break;
        }
    }

    #region Buttons
    public void Play()
    {
        GameManager.Instance.ChangeGameState(GameState.DebutGame);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {

    }

    #endregion


}
