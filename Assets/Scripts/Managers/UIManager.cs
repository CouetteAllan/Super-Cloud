using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private GameObject _buildingPanel;
    [SerializeField] private GameObject _pauseGO;
    [SerializeField] private GameObject _mainMenuGO;
    [SerializeField] private GameObject _creditsGO;
    [SerializeField] private GameObject _mainMenuBackground;
    [SerializeField] private GameObject _victoryGO;
    [SerializeField] private GameObject _gameOverGO;

    [Header("Button GamepadFocus")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _pauseResumeButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _victoryButton;
    [SerializeField] private Button _gameOverButton;

    private Animator _pauseAnimator;

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
        _pauseAnimator = _pauseGO.GetComponent<Animator>();
    }

    private void OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                DisplayPanel(PanelType.MainMenu);
                _buildingPanel.SetActive(false);
                _pauseAnimator.SetTrigger("Close");
                break;
            case GameState.DebutGame:
                _mainMenuGO.SetActive(false);


                break;
            case GameState.InGame:
                DisplayPanel(PanelType.None);
                _pauseAnimator.SetTrigger("Close");
                //Hide pause panel
                break;
            case GameState.PauseGame:
                DisplayPanel(PanelType.Pause);
                _pauseAnimator.SetTrigger("Open");
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
                _mainMenuBackground.SetActive(true);
                _mainMenuBackground.GetComponent<FadeImageTitle>().Revert();
                _pauseGO.SetActive(false);
                _gameOverGO.SetActive(false);
                _victoryGO.SetActive(false);
                _timerPanel.SetActive(false);
                _buildingPanel.SetActive(false);
                _playButton.Select();
                break;
            case PanelType.Pause:
                _pauseGO.SetActive(true);
                _mainMenuGO.SetActive(false);
                _gameOverGO.SetActive(false);
                _victoryGO.SetActive(false);
                _timerPanel.SetActive(false);
                _pauseResumeButton.Select();
                break;
            case PanelType.Victory:
                _victoryGO.SetActive(true);
                _mainMenuGO.SetActive(false);
                _pauseGO.SetActive(false);
                _gameOverGO.SetActive(false);
                _timerPanel.SetActive(false);
                _victoryButton.Select();
                break;
            case PanelType.GameOver:
                _gameOverGO.SetActive(true);
                _mainMenuGO.SetActive(false);
                _pauseGO.SetActive(false);
                _victoryGO.SetActive(false);
                _gameOverButton.Select();

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
        _mainMenuGO.SetActive(false);
        _creditsGO.SetActive(true);
        _backButton.Select();
    }

    public void Back()
    {
        _mainMenuGO.SetActive(true);
        _creditsGO.SetActive(false);
        _playButton.Select();
    }

    public void Resume()
    {
        GameManager.Instance.ChangeGameState(GameState.InGame);
    }

    public void MainMenu()
    {
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
    }

    #endregion


}
