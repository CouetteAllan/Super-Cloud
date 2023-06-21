using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class RainParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AnimationCurve rainAmountInTime;
    private ParticleSystem.EmissionModule emissionModule;

    private int _currentTick;

    private void Start()
    {
        emissionModule = particles.emission;
        TimeTickSystemDataHandler.OnTick += Tick;
    }

    private void Tick(uint tick)
    {
        _currentTick++;
        emissionModule.rateOverTime = rainAmountInTime.Evaluate(_currentTick);
    }
}
