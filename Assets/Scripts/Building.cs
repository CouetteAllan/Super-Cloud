using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Building : MonoBehaviour, IRainable
{
    [SerializeField] private GameObject[] _fire;
    [SerializeField] private int _fireTime = 15;
    [SerializeField] private float _baseFireLife = 10.0f;
    private float _fireLife;
    private int _currentTick;
    private int _currentFireIndex;
    private bool isRaining;

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
    }

    private void OnTick(uint tick)
    {
        if (isRaining)
            return;

        _currentTick++;
        if(_currentTick % (int)((_fireTime * 5)/3) == 0) //every 25 ticks, so every 5 seconds
        {
            IncreaseFire(++_currentFireIndex);
            Debug.Log("Fire !" + _currentFireIndex);
        }
    }

    private void OnTickFaster(uint tick)
    {
        if (!isRaining)
            return;

        float tickRate = 1.0f;
        _fireLife -= tickRate;
        if(_fireLife <= 0)
        {
            ChangeBuildingState(BuildingState.Normal);
        }
    }

    public void TryToGetWet(PlayerController playerController)
    {
        isRaining = true;
        if(_currentState == BuildingState.OnFire)
        {
            //Commence à éteindre le feu et diminuer la "vie" du feu au fur et à mesure qu'on pluite dessus
            //ChangeBuildingState(BuildingState.Normal);

        }
    }

    public void ChangeBuildingState(BuildingState newState)
    {
        if (newState == _currentState)
            return;
        _currentState = newState;
        switch (_currentState)
        {
            case BuildingState.Normal:
                SetFireActive(false);
                TimeTickSystemDataHandler.OnTick -= OnTick;
                TimeTickSystemDataHandler.OnTickFaster -= OnTickFaster;
                Debug.Log("JE NE SUIS PLUS EN FEU");
                BuildingManager.Instance.RemoveBuildingOnFire(this);
                break;
            case BuildingState.OnFire:
                TimeTickSystemDataHandler.OnTick += OnTick;
                TimeTickSystemDataHandler.OnTickFaster += OnTickFaster;
                _fireLife = _baseFireLife;
                IncreaseFire(0);
                break;
            case BuildingState.Destroyed:
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
}
