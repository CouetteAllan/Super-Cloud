using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BuildingManagerData", menuName = "BuildingManagerData")]
public class BuildingManagerDatas : ScriptableObject
{
    public AnimationCurve IntervalTime;
    public float MaxIntervalTime = 10;
    public float MinIntervalTime = 5;
    public AnimationCurve BuildingNumber;
    public float EarlyMaxBuildingOnFire = 2;
    public float LateMaxBuildingOnFire = 5;

    /*private void OnValidate()
    {
        IntervalTime.MoveKey(0, new Keyframe { time = 0.0f, value = MaxIntervalTime }) ;
        IntervalTime.MoveKey(1, new Keyframe { time = 180.0f, value = MinIntervalTime });

        BuildingNumber.MoveKey(0, new Keyframe { time = 0.0f, value = EarlyMaxBuildingOnFire });
        BuildingNumber.MoveKey(1, new Keyframe { time = 180.0f, value = LateMaxBuildingOnFire });
    }*/
}
