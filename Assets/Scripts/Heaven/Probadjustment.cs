using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class Probadjustment : MonoBehaviour
{
    public Image element1;
    public Text element2;
    public Image element3;
    public Button button;

    public float setProbability = 50f;  // 한 세트일 확률

    private List<string> colorNames;
    private List<string> emblemNames;
    private List<string> sortTexts;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        LoadJsonData();
    }

    private void LoadJsonData()
    {
        string colorJsonPath = "JsonFiles/Game/color";
        string emblemJsonPath = "JsonFiles/Game/emblemAssets";
        string sortJsonPath = "JsonFiles/Game/Sort";

        string colorJson = Resources.Load<TextAsset>(colorJsonPath).text;
        string emblemJson = Resources.Load<TextAsset>(emblemJsonPath).text;
        string sortJson = Resources.Load<TextAsset>(sortJsonPath).text;

        ColorData colorData = JsonUtility.FromJson<ColorData>(colorJson);
        EmblemData emblemData = JsonUtility.FromJson<EmblemData>(emblemJson);
        SortData sortData = JsonUtility.FromJson<SortData>(sortJson);

        colorNames = colorData.Color;
        emblemNames = emblemData.emblemAssets;
        sortTexts = sortData.Sort;
    }

    private void OnButtonClick()
    {
        bool isSet = IsSetElementSet();
        if (isSet)
        {
            AssignSetElements();
        }
        else
        {
            AssignNonSetElements();
        }
    }

    private bool IsSetElementSet()
    {
        float randomValue = Random.Range(0f, 100f);
        return randomValue <= setProbability;
    }

    private void AssignSetElements()
    {
        if (colorNames.Count == 0 || emblemNames.Count == 0 || sortTexts.Count == 0)
        {
            Debug.LogError("JSON 데이터가 비어 있습니다.");
            return;
        }

        int index1 = Random.Range(0, colorNames.Count);
        int index2 = Random.Range(0, sortTexts.Count);
        int index3 = Random.Range(0, emblemNames.Count);

        string colorName = colorNames[index1];
        string sortText = sortTexts[index2];
        string emblemName = emblemNames[index3];

        element1.sprite = Resources.Load<Sprite>("Image/Heaven/Train(Size)/LeftSide/" + colorName);
        element2.text = sortText;
        element3.sprite = Resources.Load<Sprite>("Image/Heaven/Train(Size)/RightSide/" + emblemName);

        if (index1 == index2 && index2 == index3)
        {
            Debug.Log("한 세트입니다.");
        }
    }

    private void AssignNonSetElements()
    {
        // 한 세트가 아닐 경우의 할당 로직을 구현해주세요.
        // 예를 들어, 각 요소에 랜덤한 값을 할당하거나 기존 값과 다른 값을 할당하는 등의 로직을 추가하시면 됩니다.
    }

    [System.Serializable]
    private class ColorData
    {
        public List<string> Color;
    }

    [System.Serializable]
    private class EmblemData
    {
        public List<string> emblemAssets;
    }

    [System.Serializable]
    private class SortData
    {
        public List<string> Sort;
    }
}
