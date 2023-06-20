using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour, IRainable
{
    public static event Action<Building> OnBuildingDestroyed;

    [SerializeField] private GameObject[] _fire;
    [SerializeField] private int _fireTime = 15;
    [SerializeField] private float _baseFireLife = 10.0f;
    [SerializeField] private Image _fireLifeImage;
    [SerializeField] private SpriteRenderer _buildingSprite;
    [SerializeField] private Sprite _ruinedBuilding;
    private bool _showFireLife;
    private float _fireLife;
    private int _currentTick;
    private int _currentFireIndex;
    private bool isRaining;
    private Color _baseColor;

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
    }

    private void OnTick(uint tick)
    {
        if (isRaining)
            return;

        _currentTick++;
        if(_currentTick % (int)((_fireTime * 5)/3) == 0) //every 25 ticks, so every 5 seconds
        {
            IncreaseFire(++_currentFireIndex);
            if (_currentFireIndex > _fire.Length)
                ChangeBuildingState(BuildingState.Destroyed);

            if(_currentFireIndex > _fire.Length - 1 && IsAlive())
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
            _fireLifeImage.transform.parent.gameObject.SetActive(false);
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
        if (this.gameObject.name.Contains("test"))
        switch (_currentState)
        {
            case BuildingState.Normal:
                SetFireActive(false);
                TimeTickSystemDataHandler.OnTick -= OnTick;
                TimeTickSystemDataHandler.OnTickFaster -= OnTickFaster;
                Debug.Log("JE NE SUIS PLUS EN FEU");
                BuildingManager.Instance.RemoveBuildingOnFire(this);
                _buildingSprite.color = _baseColor;
                break;
            case BuildingState.OnFire:
                TimeTickSystemDataHandler.OnTick += OnTick;
                TimeTickSystemDataHandler.OnTickFaster += OnTickFaster;
                _fireLife = _baseFireLife;
                IncreaseFire(0);
                break;
            case BuildingState.Destroyed:
                TimeTickSystemDataHandler.OnTick -= OnTick;
                TimeTickSystemDataHandler.OnTickFaster -= OnTickFaster;
                OnBuildingDestroyed?.Invoke(this);
                SetFireActive(false);
                _buildingSprite.color = _baseColor;
                _buildingSprite.sprite = _ruinedBuilding;
                this.enabled = false;
                break;
        }
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

        for(int i = 0; i <=  fireMaxActive; i++)
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
