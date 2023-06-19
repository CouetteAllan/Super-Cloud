using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private AnimationCurve _accelerationCurve;
    [SerializeField] private AnimationCurve _decelerationCurve;

    private Rigidbody2D _rb;
    private PlayerController _playerController;
    private float _timerCurve;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        _timerCurve += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2 targetMovement = _playerController.InputDir;

        AnimationCurve speedcurve = _playerController.IsMoving ? _accelerationCurve : _decelerationCurve;

        _rb.velocity = new Vector2(targetMovement.x * speedcurve.Evaluate(_timerCurve) * _speed, targetMovement.y * speedcurve.Evaluate(_timerCurve) * _speed);
    }

    public void UpdateCurveForTimer(float time)
    {
        _timerCurve = time;
    }
}
