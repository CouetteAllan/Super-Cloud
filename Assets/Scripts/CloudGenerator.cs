using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{

    [SerializeField] ParticleSystem _cloud;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<WaterHandle>(out WaterHandle player))
        {
            player.Refuel();
            var parameter = _cloud.emission;
            parameter.rateOverTime = 5;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<WaterHandle>(out WaterHandle player))
        {
            player.ExitRefuel();
            var parameter = _cloud.emission;
            parameter.rateOverTime = 2;

        }
    }
}
