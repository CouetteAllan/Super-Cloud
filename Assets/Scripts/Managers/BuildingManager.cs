using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] private BuildingManagerDatas _datas;
    public List<Building> Buildings { get; private set; } = new List<Building>();
    public List<Building> BuildingsOnFire { get; private set; } = new List<Building>();
    public List<Building> BuildingsDestroyed { get; private set; } = new List<Building>();

    private int _nbBuildingOnFire = 0;
    private float _timerNextBuilding = 0;
    private bool _canFireBuilding = false;
    private float _elapsedTime = 0.0f;
    private bool _twice = false;

    private void Start()
    {
        var goBuildings = GameObject.FindGameObjectsWithTag("Building");
        foreach (var building in goBuildings)
        {
            Buildings.Add(building.GetComponent<Building>());
        }

        foreach (var building in Buildings)
        {
            building.ChangeBuildingState(Building.BuildingState.Normal);
        }

        TimeTickSystemDataHandler.OnTick += OnTick;
        Building.OnBuildingDestroyed += OnBuildingDestroyed;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Victory || state == GameState.MainMenu)
        {

            foreach (var building in BuildingsDestroyed)
            {
                building.ChangeBuildingState(Building.BuildingState.Normal);
                Buildings.Add(building);
            }

            BuildingsOnFire.Clear();
            BuildingsDestroyed.Clear();
        }

        if (state == GameState.DebutGame)
        {
            _elapsedTime = 0.0f;
            _twice = false;
        }
    }

    private void OnBuildingDestroyed(Building building)
    {
        BuildingsOnFire.Remove(building);
        BuildingsDestroyed.Add(building);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.InGame)
            return;
        _elapsedTime += Time.deltaTime;

        if (BuildingsOnFire.Count >= Mathf.CeilToInt(_datas.BuildingNumber.Evaluate(_elapsedTime)) || _canFireBuilding)
            return;

        _timerNextBuilding += Time.deltaTime;
        if (_timerNextBuilding > _datas.IntervalTime.Evaluate(_elapsedTime))
        {
            _canFireBuilding = true;
            _timerNextBuilding -= _datas.IntervalTime.Evaluate(_elapsedTime);
        }
        if (_elapsedTime > 135.0f)
            _twice = true;
    }

    private void OnTick(uint tick)
    {
        if (!_canFireBuilding) return;

        FireRandomBuilding();
        _canFireBuilding = false;
    }

    private void FireRandomBuilding()
    {
        int nb = _twice ? 2 : 1;
        for (int i = 0; i < nb; i++)
        {
            var randomIndex = Random.Range(0, Buildings.Count - 1);
            BuildingsOnFire.Add(Buildings[randomIndex]);
            Buildings[randomIndex].ChangeBuildingState(Building.BuildingState.OnFire);
            Buildings.RemoveAt(randomIndex);
        }
    }

    public void RemoveBuildingOnFire(Building building)
    {
        BuildingsOnFire.Remove(building);
        Buildings.Add(building);
    }

    private void OnDisable()
    {
        TimeTickSystemDataHandler.OnTick -= OnTick;
        Building.OnBuildingDestroyed -= OnBuildingDestroyed;
        GameManager.OnGameStateChanged -= OnGameStateChanged;

    }
}
