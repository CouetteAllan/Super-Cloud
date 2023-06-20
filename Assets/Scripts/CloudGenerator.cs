using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<WaterHandle>(out WaterHandle player))
        {
            player.Refuel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<WaterHandle>(out WaterHandle player))
        {
            player.ExitRefuel();
        }
    }
}
