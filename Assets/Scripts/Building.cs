using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Building : MonoBehaviour, IRainable
{
    [SerializeField] private GameObject[] _fire;
    private int _currentTick;
    private int _currentFireIndex;

    public enum BuildingState
    {
        Normal,
        OnFire,
        Destroyed
    }

    private BuildingState _currentState = BuildingState.Normal;
    
    private void Start()
    {
        ChangeBuildingState(BuildingState.OnFire);
    }

    private void OnTick(int tick)
    {
        _currentTick++;
        if(_currentTick % 25 == 0) //every 25 ticks, so every 5 seconds
        {
            IncreaseFire(++_currentFireIndex);
            Debug.Log("Fire !" + _currentFireIndex);
        }
    }

    public void TryToGetWet(PlayerController playerController)
    {
        if(_currentState == BuildingState.OnFire)
        {
            //Commence à éteindre le feu et diminuer la "vie" du feu au fur et à mesure qu'on pluite dessus
            ChangeBuildingState(BuildingState.Normal);

        }
    }

    private void ChangeBuildingState(BuildingState newState)
    {
        if (newState == _currentState)
            return;
        _currentState = newState;
        switch (_currentState)
        {
            case BuildingState.Normal:
                SetFireActive(false);
                TimeTickSystemDataHandler.OnTick -= OnTick;
                Debug.Log("JE NE SUIS PLUS EN FEU");
                break;
            case BuildingState.OnFire:
                TimeTickSystemDataHandler.OnTick += OnTick;
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
}
