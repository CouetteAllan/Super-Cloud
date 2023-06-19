using Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
        _playerMovement = GetComponent<PlayerMovement>();

        _playerActions = new MPlayerInputActions();
        _playerActions.Player.Rain.performed += Rain_performed;
        _playerActions.Player.Move.started += Move_started;
        _playerActions.Player.Move.canceled += Move_canceled;
        _playerActions.Enable();
    }

    private void Move_canceled(InputAction.CallbackContext context) => _playerMovement.UpdateCurveForTimer(0.0f);

    private void Move_started(InputAction.CallbackContext context) => _playerMovement.UpdateCurveForTimer(0.0f);

    private void Rain_performed(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
