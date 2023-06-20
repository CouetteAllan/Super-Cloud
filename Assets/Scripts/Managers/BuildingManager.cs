using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private BuildingManagerDatas _datas;
    public List<Building> Buildings { get; private set; } = new List<Building>();

    private void Start()
    {
        var goBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (var building in goBuildings)
        {
            Buildings.Add(building.GetComponent<Building>());
        }

        foreach(var building in Buildings)
        {
            building.ChangeBuildingState(Building.BuildingState.Normal);
        }

        TimeTickSystemDataHandler.OnTick += OnTick;
    }

    private void OnTick(uint tick)
    {
        throw new System.NotImplementedException();
    }
}
