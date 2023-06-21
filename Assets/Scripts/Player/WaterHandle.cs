using UnityEngine;

public class WaterHandle : MonoBehaviour
{
    [SerializeField] private float _maxWater = 100.0f;
    [SerializeField] private float _drainWaterPerTick = 1.0f;
    [SerializeField] private float _refillWaterPerTick = 10.0f;
    [SerializeField] private Transform _graphTransform;
    [SerializeField] private Transform _rainParticleTransform;
    [SerializeField] private AnimationCurve _speedChangeCurve;

    private PlayerMovement _playerMovement;
    private float _currentWater;
    private float _baseScale = 2.0f;
    private bool _onTopOfRefill = false;
    private float _baseSpeed;

    private enum WaterState
    {
        Using,
        Refilling,
        None
    }
    private WaterState _state = WaterState.None;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        WaterDataHandler.OnUsingWater += OnUsingWater;
        WaterDataHandler.OnStopUsingWater += OnStopUsingWater;
        TimeTickSystemDataHandler.OnTick += OnTick;

        _currentWater = _maxWater;
        _baseSpeed = _playerMovement.Speed;
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
                    _state = WaterState.None;
                }
                var currentScaleMinus = _graphTransform.localScale;
                currentScaleMinus.x = Mathf.Clamp(_graphTransform.localScale.x - (((_baseScale / _maxWater) / 2) * _drainWaterPerTick), 1.0f, 2.0f);
                currentScaleMinus.y = Mathf.Clamp(_graphTransform.localScale.y - (((_baseScale / _maxWater) / 2) * _drainWaterPerTick), 1.0f, 2.0f);
                _graphTransform.localScale = currentScaleMinus;
                _rainParticleTransform.localScale = currentScaleMinus;
                _playerMovement.Speed = _speedChangeCurve.Evaluate(1 - (_currentWater / _maxWater));
                break;

            case WaterState.Refilling:
                _currentWater = Mathf.Clamp(_currentWater + _refillWaterPerTick, 0.0f, _maxWater);
                var currentScalePlus = _graphTransform.localScale;
                currentScalePlus.x = Mathf.Clamp(_graphTransform.localScale.x + (((_baseScale / _maxWater) / 2) * _refillWaterPerTick), 1.0f, 2.0f);
                currentScalePlus.y = Mathf.Clamp(_graphTransform.localScale.y + (((_baseScale / _maxWater) / 2) * _refillWaterPerTick), 1.0f, 2.0f);
                _graphTransform.localScale = currentScalePlus;
                _rainParticleTransform.localScale = currentScalePlus;
                SoundManager.Instance.Play("Boing");
                Debug.Log("Water Level: " + _currentWater);
                _playerMovement.Speed = _speedChangeCurve.Evaluate(1 - (_currentWater / _maxWater));
                if (_currentWater >= _maxWater)
                {
                    this.WaterFull();
                    _state = WaterState.None;
                    _graphTransform.localScale = new Vector3(2.0f,2.0f);
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
        if (_state == WaterState.Using)
            return;
        _state = WaterState.Refilling;
        _onTopOfRefill = true;
        if(_currentWater < 0)
            _currentWater = 0;
        this.WaterRefilling();

    }

    public void ExitRefuel()
    {
        if (_state == WaterState.Using)
            return;
        _state = WaterState.None;
        _onTopOfRefill = false;
        this.StopWaterRefilling();
    }

    private void OnDisable()
    {
        WaterDataHandler.OnUsingWater -= OnUsingWater;
        WaterDataHandler.OnStopUsingWater -= OnStopUsingWater;
        TimeTickSystemDataHandler.OnTick -= OnTick;

    }
}
