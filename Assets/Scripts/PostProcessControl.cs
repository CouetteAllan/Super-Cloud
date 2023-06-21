using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessControl : MonoBehaviour
{
    [SerializeField] VolumeProfile _baseProfile;
    [SerializeField] VolumeProfile _thunderProfile;
    [SerializeField] CinemachineVolumeSettings _volumeSettings;

    private Coroutine _thunderCoroutine;

    private void Start()
    {
        Building.OnBuildingFire += Building_OnBuildingFire;
    }

    private void Building_OnBuildingFire(Building building)
    {
        if (_thunderCoroutine != null)
            StopCoroutine(_thunderCoroutine);

        _thunderCoroutine = StartCoroutine(ThunderCoroutine());
    }

    IEnumerator ThunderCoroutine()
    {
        _volumeSettings.m_Profile = _thunderProfile;
        Debug.Log(_volumeSettings.m_Profile);
        yield return new WaitForSeconds(0.1f);
        _volumeSettings.m_Profile = _baseProfile;
        Debug.Log(_volumeSettings.m_Profile);

    }

    private void OnDisable()
    {
        Building.OnBuildingFire -= Building_OnBuildingFire;
    }
}
