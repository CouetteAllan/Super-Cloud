using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{
    private const float TICK_TIMER_MAX = 0.2f;

    private int _tick = 0;
    private float _tickTimer;

    private void Awake()
    {
        _tickTimer = 0.0f;
    }

    private void Update()
    {
        _tickTimer += Time.deltaTime;
        if(_tickTimer > TICK_TIMER_MAX)
        {
            _tickTimer -= TICK_TIMER_MAX;
            _tick++;
            this.Tick(_tick);
        }
    }
}
