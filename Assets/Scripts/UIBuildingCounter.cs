using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildingCounter : MonoBehaviour
{
    [SerializeField] private Sprite[] buildingsSprites;
    [SerializeField] private Sprite destroyedBuildingSprite;
    [SerializeField] private HorizontalLayoutGroup buildingsImageGroup;

    [SerializeField] private GameObject buildingImage_PF;

    private Image[] buildingImages = new Image[0];
    private int currentBuildingImgIndex = 0;

    private void Reset()
    {
        buildingsImageGroup = this.GetComponent<HorizontalLayoutGroup>();
    }

    private void Start()
    {
        if (GameManager.Instance == null) return;
        SetUI();

        Building.OnBuildingDestroyed += DestroyBuildingInUI;
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        this.gameObject.SetActive(false);

    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            SetUI();
        }
    }

    private void SetUI()
    {
        foreach (var item in buildingImages)
        {
            Destroy(item.gameObject);
        }

        int maxBuildings = GameManager.Instance.MaxBuildingDestroyed;
        buildingImages = new Image[maxBuildings];
        for (int i = 0; i < GameManager.Instance.MaxBuildingDestroyed; i++)
        {
            GameObject go = Instantiate(buildingImage_PF, buildingsImageGroup.transform);
            Image goImg = go.GetComponent<Image>();
            goImg.sprite = buildingsSprites[UnityEngine.Random.Range(0, buildingsSprites.Length)];
            buildingImages[i] = goImg;
        }
        currentBuildingImgIndex = 0;
    }

    private void DestroyBuildingInUI(Building b)
    {
        if (buildingImages.Length <= 0 || currentBuildingImgIndex >= buildingImages.Length) return;
        buildingImages[currentBuildingImgIndex].sprite = destroyedBuildingSprite;
        currentBuildingImgIndex++;
    }

    private void OnDestroy()
    {
        Building.OnBuildingDestroyed -= DestroyBuildingInUI;
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }
}
