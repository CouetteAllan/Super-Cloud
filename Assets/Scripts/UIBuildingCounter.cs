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

    private Stack<Image> buildingImages = new Stack<Image>();

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
            //SetUI();
        }
        if(state == GameState.InGame) SetUI();
    }

    private void SetUI()
    {
        int INFINITELOOPFAILSAFE = 100;
        while (buildingImages.Count > 0 && INFINITELOOPFAILSAFE > 0)
        {
            INFINITELOOPFAILSAFE--;
            Destroy(buildingImages.Pop().gameObject);
        }

        buildingImages = new Stack<Image>();
        for (int i = 0; i < GameManager.Instance.MaxBuildingDestroyed; i++)
        {
            GameObject go = Instantiate(buildingImage_PF, buildingsImageGroup.transform);
            Image goImg = go.GetComponent<Image>();
            goImg.sprite = buildingsSprites[UnityEngine.Random.Range(0, buildingsSprites.Length)];
            buildingImages.Push(goImg);
        }
    }

    private void DestroyBuildingInUI(Building b)
    {
        if (buildingImages.Count <= 0) return;
        buildingImages.Pop().sprite = destroyedBuildingSprite;
    }

    private void OnDestroy()
    {
        Building.OnBuildingDestroyed -= DestroyBuildingInUI;
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }
}
