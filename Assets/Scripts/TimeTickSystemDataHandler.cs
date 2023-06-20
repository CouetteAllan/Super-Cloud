using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TimeTickSystemDataHandler
{
    public static event Action<int> OnTick;
    public static void Tick(this TimeTickSystem timeTickSystem, int tick) => OnTick?.Invoke(tick);
}
