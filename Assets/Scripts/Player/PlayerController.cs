using Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _rain;
    [SerializeField] private ParticleSystem _rainParticle;
    [SerializeField] private BoxCollider2D _rainCollision;
    [SerializeField] private LayerMask _layerRain;
    private bool _canRain = true;

    private Animator _animator;

    public Vector2 InputDir { get
        {
            if (_playerActions == null)
                return Vector2.zero;

            return _playerActions.Player.Move.ReadValue<Vector2>();
        }
    }
    public bool IsMoving => _lastInput != Vector2.zero;

    private Vector2 _lastInput { get
        {
            if( _playerActions == null)
                return Vector2.zero;

            var input = _playerActions.Player.Move.ReadValue<Vector2>();
            if (Mathf.Abs(input.x) > 0.01f || Mathf.Abs(input.y) > 0.01f)
                return input;
            else 
                return Vector2.zero;
        } }
    private MPlayerInputActions _playerActions;
    private PlayerMovement _playerMovement;


    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rain.SetActive(false);
        _playerMovement = GetComponent<PlayerMovement>();
        WaterDataHandler.OnWaterEmpty += OnWaterEmpty;
        WaterDataHandler.OnWaterRefilling += OnWaterRefilling;

        _playerActions = new MPlayerInputActions();
        _playerActions.Player.Rain.started += Rain_started;
        _playerActions.Player.Rain.canceled += Rain_canceled;
        _playerActions.Player.Move.started += Move_started;
        _playerActions.Player.Move.canceled += Move_canceled;
        _playerActions.Player.Move.performed += Move_performed;
        _playerActions.Enable();
    }

    private void OnWaterRefilling()
    {
        _canRain = true;
    }

    private void OnWaterEmpty()
    {
        _canRain = false;
        Rain_canceled(new InputAction.CallbackContext { });
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        _animator.SetFloat("Dir", _playerActions.Player.Move.ReadValue<Vector2>().x);
    }

    private void Move_started(InputAction.CallbackContext context) => _playerMovement.UpdateCurveForTimer(0.0f);

    private void Move_canceled(InputAction.CallbackContext context) => _playerMovement.UpdateCurveForTimer(0.0f);


    private void Rain_started(InputAction.CallbackContext context)
    {
        if (_rain.activeSelf == false && _canRain)
            DoRain();
        
    }
    private void Rain_canceled(InputAction.CallbackContext context)
    {
        StopRain();
    }

    private void DoRain()
    {
        this.UsingWater();
        _rain.SetActive(true);
        _animator.SetBool("IsRaining", true);
        _rainParticle.Play();
    }

    private void StopRain()
    {
        this.StopUsingWater();
        _rain.SetActive(false);
        _animator.SetBool("IsRaining", false);
        _rainParticle.Stop();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.TryGetComponent<IRainable>(out  IRainable rainable) && _rainCollision.IsTouchingLayers(_layerRain))
        {
            rainable.TryToGetWet(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IRainable>(out IRainable rainable) && !_rainCollision.IsTouchingLayers(_layerRain))
        {
            rainable.StopGettingWet(this);
        }
    }

    private void OnDisable()
    {
        _playerActions.Player.Rain.started -= Rain_started;
        _playerActions.Player.Rain.canceled -= Rain_canceled;
        _playerActions.Player.Move.started -= Move_started;
        _playerActions.Player.Move.canceled -= Move_canceled;
        WaterDataHandler.OnWaterRefilling -= OnWaterRefilling;
        WaterDataHandler.OnWaterEmpty -= OnWaterEmpty;


    }
}
