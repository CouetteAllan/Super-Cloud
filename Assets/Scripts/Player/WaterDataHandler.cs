using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaterDataHandler
{
    public static event Action OnUsingWater;
    public static event Action OnStopUsingWater;
    public static event Action OnWaterEmpty;
    public static event Action OnWaterFull;
    public static event Action OnWaterRefilling;

    public static void UsingWater(this PlayerController playerController) => OnUsingWater?.Invoke();
    public static void StopUsingWater(this PlayerController player) => OnStopUsingWater?.Invoke();
    public static void WaterEmpty(this WaterHandle waterHandle) => OnWaterEmpty?.Invoke();
    public static void WaterFull(this WaterHandle waterHandle) => OnWaterFull?.Invoke();
    public static void WaterRefilling(this WaterHandle waterHandle) => OnWaterRefilling?.Invoke();

}
