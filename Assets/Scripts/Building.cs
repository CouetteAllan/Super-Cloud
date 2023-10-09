using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour, IRainable
{
    public static event Action<Building> OnBuildingDestroyed;
    public static event Action<Building> OnBuildingFire;

    [SerializeField] private GameObject[] _fire;
    [SerializeField] private int _fireTime = 15;
    [SerializeField] private float _baseFireLife = 10.0f;
    [SerializeField] private Image _fireLifeImage;
    [SerializeField] private SpriteRenderer _buildingSprite;
    [SerializeField] private Sprite _ruinedBuilding;
    [SerializeField] private AudioSource _buildingAudioSource;
    [SerializeField] private ParticleSystem _buildingSmoke;
    [SerializeField] private Animator _thunder;

    private bool _showFireLife;
    private float _fireLife;
    private int _currentTick;
    private int _currentFireIndex;
    private bool isRaining;
    private Color _baseColor;
    private Sprite _baseSprite;

    public enum BuildingState
    {
        Normal,
        OnFire,
        Destroyed
    }

    private BuildingState _currentState = BuildingState.Normal;

    private void Start()
    {
        //ChangeBuildingState(BuildingState.OnFire);
        _baseColor = _buildingSprite.color;
        _baseSprite = _buildingSprite.sprite;
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        if(state == GameState.MainMenu || state == GameState.Victory)
        {
            ChangeBuildingState(BuildingState.Normal);
        }
    }

    private void OnTick(uint tick)
    {
        if (isRaining)
            return;

        _currentTick++;
        if (_currentTick % (int)((_fireTime * 5) / 3) == 0) //every 25 ticks, so every 5 seconds
        {
            IncreaseFire(++_currentFireIndex);
            if (_currentFireIndex > _fire.Length)
                ChangeBuildingState(BuildingState.Destroyed);

            if (_currentFireIndex > _fire.Length - 1 && IsAlive())
            {
                //enclencher feedback de "bientot dead"
                _buildingSprite.color = Color.red;
            }
        }
    }

    private void OnTickFaster(uint tick)
    {
        if (!isRaining)
            return;

        float tickRate = 1.0f;
        _fireLife -= tickRate;
        _fireLifeImage.transform.parent.gameObject.SetActive(true);
        _fireLifeImage.fillAmount = 1 - (_fireLife / _baseFireLife);
        if (_fireLife <= 0)
        {
            ChangeBuildingState(BuildingState.Normal);
        }
    }

    public void TryToGetWet(PlayerController playerController)
    {
        isRaining = true;
    }

    public void ChangeBuildingState(BuildingState newState)
    {
        if (newState == _currentState)
            return;
        _currentState = newState;
        switch (_currentState)
        {
            case BuildingState.Normal:
                NormalState();
                break;

            case BuildingState.OnFire:
                FireState();
                break;

            case BuildingState.Destroyed:
                DestroyedState();
                break;
        }
    }

    private void NormalState()
    {
        SetFireActive(false);
        _fireLifeImage.transform.parent.gameObject.SetActive(false);
        TimeTickSystemDataHandler.OnTick -= OnTick;
        TimeTickSystemDataHandler.OnTickFaster -= OnTickFaster;
        BuildingManager.Instance.RemoveBuildingOnFire(this);
        _buildingSprite.color = _baseColor;
        _buildingSprite.sprite = _baseSprite;
        _buildingAudioSource.Stop();
        _buildingSmoke.Stop();
    }

    private void FireState()
    {
        _currentFireIndex = 0;
        _thunder.SetTrigger("Thunder");
        TimeTickSystemDataHandler.OnTick += OnTick;
        TimeTickSystemDataHandler.OnTickFaster += OnTickFaster;
        SoundManager.Instance.Play("Thunder");
        _buildingAudioSource.Play();
        _buildingSmoke.Play();
        _fireLife = _baseFireLife;
        IncreaseFire(0);
        OnBuildingFire?.Invoke(this);
    }

    private void DestroyedState()
    {
        TimeTickSystemDataHandler.OnTick -= OnTick;
        TimeTickSystemDataHandler.OnTickFaster -= OnTickFaster;
        OnBuildingDestroyed?.Invoke(this);
        SetFireActive(false);
        _fireLifeImage.transform.parent.gameObject.SetActive(false);
        _buildingSprite.color = _baseColor;
        _buildingSprite.sprite = _ruinedBuilding;
        this.enabled = false;
        _buildingAudioSource.Stop();
        _buildingSmoke.Stop();
    }

    private void SetFireActive(bool active)
    {
        foreach (var fire in _fire)
        {
            fire.SetActive(active);
        }
    }

    private void IncreaseFire(int fireMaxActive)
    {
        if (fireMaxActive > _fire.Length - 1)
            return;

        for (int i = 0; i <= fireMaxActive; i++)
        {
            _fire[i].SetActive(true);
        }
    }

    public void StopGettingWet(PlayerController playerController)
    {
        isRaining = false;
    }

    private bool IsAlive() => _currentFireIndex <= _fire.Length;

    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= OnTick;
        TimeTickSystemDataHandler.OnTickFaster -= OnTickFaster;
    }
}
