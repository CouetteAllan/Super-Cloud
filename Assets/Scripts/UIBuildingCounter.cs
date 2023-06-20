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

    private Stack<Image> buildingImages;

    private void Reset()
    {
        buildingsImageGroup = this.GetComponent<HorizontalLayoutGroup>();
    }

    private void Start()
    {
        if (GameManager.Instance == null) return;
        buildingImages = new Stack<Image>();
        for (int i = 0; i < GameManager.Instance.MaxBuildingDestroyed; i++)
        {
            GameObject go = Instantiate(buildingImage_PF, buildingsImageGroup.transform);
            Image goImg = go.GetComponent<Image>();
            goImg.sprite = buildingsSprites[Random.Range(0, buildingsSprites.Length)];
            buildingImages.Push(goImg);
        }
        Building.OnBuildingDestroyed += DestroyBuildingInUI;
        this.gameObject.SetActive(false);
    }

    private void DestroyBuildingInUI(Building b)
    {
        if (buildingImages.Count <= 0) return;
        buildingImages.Pop().sprite = destroyedBuildingSprite;
    }
}
