using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Building : MonoBehaviour, IRainable
{
    [SerializeField] private GameObject _fire;

    public enum BuildingState
    {
        Normal,
        OnFire,
        Destroyed
    }

    private BuildingState _currentState = BuildingState.Normal;
    
    private void Start()
    {
        _fire.SetActive(false);
    }

    public void TryToGetWet(PlayerController playerController)
    {
        if(_currentState == BuildingState.OnFire)
        {
            //Commence à éteindre le feu et diminuer la "vie" du feu au fur et à mesure qu'on pluite dessus

        }
    }
}
