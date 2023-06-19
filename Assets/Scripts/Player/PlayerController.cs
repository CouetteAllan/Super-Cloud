using Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _rain;

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
        _rain.SetActive(false);
        _playerMovement = GetComponent<PlayerMovement>();

        _playerActions = new MPlayerInputActions();
        _playerActions.Player.Rain.started += Rain_started;
        _playerActions.Player.Rain.canceled += Rain_canceled;
        _playerActions.Player.Move.started += Move_started;
        _playerActions.Player.Move.canceled += Move_canceled;
        _playerActions.Enable();
    }

    
    private void Move_started(InputAction.CallbackContext context) => _playerMovement.UpdateCurveForTimer(0.0f);

    private void Move_canceled(InputAction.CallbackContext context) => _playerMovement.UpdateCurveForTimer(0.0f);


    private void Rain_started(InputAction.CallbackContext context)
    {
        if (_rain.activeSelf == false)
            DoRain();
        
    }
    private void Rain_canceled(InputAction.CallbackContext context)
    {
        StopRain();
    }

    private void DoRain()
    {
        _rain.SetActive(true);
    }

    IEnumerator CheckRainCoroutine()
    {
        yield return null;
    }

    private void StopRain()
    {
        _rain.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IRainable>(out  IRainable rainable))
        {
            rainable.TryToGetWet(this);
        }
    }
}
