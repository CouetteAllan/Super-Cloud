using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRainable
{
    public void TryToGetWet(PlayerController playerController);
    public void StopGettingWet(PlayerController playerController);
}
