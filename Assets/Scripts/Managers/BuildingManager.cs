using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private BuildingManagerDatas _datas;
    public List<Building> Buildings { get; private set; } = new List<Building>();
    public List<Building> BuildingsOnFire { get; private set; } = new List<Building>();

    private int _nbBuildingOnFire = 0;
    private float _timerNextBuilding = 0;
    private bool _canFireBuilding = false;

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

    private void Update()
    {
        if (BuildingsOnFire.Count >= Mathf.CeilToInt(_datas.BuildingNumber.Evaluate(Time.realtimeSinceStartup)) || _canFireBuilding)
            return;

        _timerNextBuilding += Time.deltaTime;
        if(_timerNextBuilding > _datas.IntervalTime.Evaluate(Time.realtimeSinceStartup))
        {
            _canFireBuilding = true;
            _timerNextBuilding -= _datas.IntervalTime.Evaluate(Time.realtimeSinceStartup);
        }
    }

    private void OnTick(uint tick)
    {
        if(!_canFireBuilding) return;

        FireRandomBuilding();
        _canFireBuilding = false;
    }

    private void FireRandomBuilding()
    {
        var randomIndex = Random.Range(0,Buildings.Count - 1);
        BuildingsOnFire.Add(Buildings[randomIndex]);
        Buildings[randomIndex].ChangeBuildingState(Building.BuildingState.OnFire);
        Buildings.RemoveAt(randomIndex);
    }

    public void RemoveBuildingOnFire(Building building)
    {
        BuildingsOnFire.Remove(building);
        Buildings.Add(building);
    }

    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= OnTick;
    }
}
