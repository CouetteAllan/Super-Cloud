using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class WaterHandle : MonoBehaviour
{
    [SerializeField] private float _maxWater = 100.0f;
    [SerializeField] private float _drainWaterPerTick = 1.0f;
    [SerializeField] private float _refillWaterPerTick = 10.0f;
    [SerializeField] private Transform _graphTransform;
    private float _currentWater;
    private float _targetScale = 1.0f;
    private float _baseScale = 2.0f;
    private bool _onTopOfRefill = false;

    private enum WaterState
    {
        Using,
        Refilling,
        None
    }
    private WaterState _state;

    private void Start()
    {
        WaterDataHandler.OnUsingWater += OnUsingWater;
        WaterDataHandler.OnStopUsingWater += OnStopUsingWater;
        TimeTickSystemDataHandler.OnTick += OnTick;

        _currentWater = _maxWater;
    }

    private void OnUsingWater()
    {
        _state = WaterState.Using;
    }

    private void OnTick(uint tick)
    {
        switch (_state)
        {
            case WaterState.Using:
                _currentWater -= _drainWaterPerTick;
                Debug.Log("Water Level: " + _currentWater);
                if (_currentWater <= 0)
                {
                    this.WaterEmpty();
                    TimeTickSystemDataHandler.OnTick -= OnTick;
                }
                var currentScaleMinus = _graphTransform.localScale;
                currentScaleMinus.x = Mathf.Clamp(_graphTransform.localScale.x - (((_baseScale / _maxWater) / 2) * _drainWaterPerTick), 1.0f, 2.0f);
                currentScaleMinus.y = Mathf.Clamp(_graphTransform.localScale.y - (((_baseScale / _maxWater) / 2) * _drainWaterPerTick), 1.0f, 2.0f);
                _graphTransform.localScale = currentScaleMinus;
                break;

            case WaterState.Refilling:
                _currentWater = Mathf.Clamp(_currentWater + _refillWaterPerTick, 0.0f, _maxWater);
                var currentScalePlus = _graphTransform.localScale;
                currentScalePlus.x = Mathf.Clamp(_graphTransform.localScale.x + (((_baseScale / _maxWater) / 2) * _refillWaterPerTick), 1.0f, 2.0f);
                currentScalePlus.y = Mathf.Clamp(_graphTransform.localScale.y + (((_baseScale / _maxWater) / 2) * _refillWaterPerTick), 1.0f, 2.0f);
                _graphTransform.localScale = currentScalePlus;
                Debug.Log("Water Level: " + _currentWater);
                if (_currentWater >= _maxWater)
                {
                    this.WaterFull();
                    _state = WaterState.None;
                }
                break;

        }
       
    }

    private void OnStopUsingWater()
    {
        if (_onTopOfRefill)
            Refuel();
        else
            _state = WaterState.None;

    }


    public void Refuel()
    {
        _state = WaterState.Refilling;
        _onTopOfRefill = true;
        if(_currentWater < 0)
            _currentWater = 0;
        this.WaterRefilling();

    }

    public void ExitRefuel()
    {
        _state = WaterState.None;
        _onTopOfRefill = false;
    }

    private void OnDisable()
    {
        WaterDataHandler.OnUsingWater -= OnUsingWater;
        WaterDataHandler.OnStopUsingWater -= OnStopUsingWater;
        TimeTickSystemDataHandler.OnTick -= OnTick;

    }
}
