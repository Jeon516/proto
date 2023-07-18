using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class AssignElementsData
{
    public List<string> color;
    public List<string> emblemAssets;
    public List<string> sort;
}

public class TrainElement : MonoBehaviour
{
    public Text textElement;
    public Image imageElement;

    public string jsonFilePath = "JsonFiles/Game/AssignElementsData.json";

    private AssignElementsData elementsData;
    private int currentIndex = 0; // 현재 세트의 인덱스

    private void Start()
    {
        LoadMappingData();
        AssignElementsBasedOnIndex();
    }

    private void LoadMappingData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
        string jsonContent = File.ReadAllText(filePath);
        elementsData = JsonUtility.FromJson<AssignElementsData>(jsonContent);
    }

    private void AssignElementsBasedOnIndex()
    {
        if (currentIndex < 0 || currentIndex >= elementsData.color.Count)
        {
            Debug.LogWarning("Invalid index: " + currentIndex);
            return;
        }

        string emblemAssetName = elementsData.emblemAssets[currentIndex];
        string sortValue = elementsData.sort[currentIndex];

        AssignEmblemAsset(emblemAssetName);
        AssignSort(sortValue);
    }

    private void AssignEmblemAsset(string emblemAssetName)
    {
        // emblemAssetName에 따라 적절한 이미지를 할당합니다.
        // 예시로 이미지 요소에 이미지를 할당합니다.
        if (imageElement != null)
        {
            string imagePath = "Image/Emblems/" + emblemAssetName;
            Sprite assignedSprite = Resources.Load<Sprite>(imagePath);
            if (assignedSprite != null)
            {
                imageElement.sprite = assignedSprite;
            }
        }
    }

    private void AssignSort(string sortValue)
    {
        // sortValue에 따라 적절한 텍스트를 할당합니다.
        // 예시로 텍스트 요소에 문자열을 할당합니다.
        if (textElement != null)
        {
            textElement.text = sortValue;
        }
    }

    // 다음 세트로 이동하는 함수
    public void NextSet()
    {
        currentIndex++;
        if (currentIndex >= elementsData.color.Count)
        {
            currentIndex = 0;
        }

        AssignElementsBasedOnIndex();
    }

    // 이전 세트로 이동하는 함수
    public void PreviousSet()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = elementsData.color.Count - 1;
        }

        AssignElementsBasedOnIndex();
    }
}
