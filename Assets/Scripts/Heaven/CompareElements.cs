using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CompareElements : MonoBehaviour
{
    public Image element1;
    public Text element2;
    public Image element3;
    public Button compareButton;

    private string colorJsonPath = "JsonFiles/Game/color";
    private string sortJsonPath = "JsonFiles/Game/Sort";
    private string emblemJsonPath = "JsonFiles/Game/emblemAssets";

    private List<string> colorNames;
    private List<string> sortTexts;
    private List<string> emblemNames;

    private void Start()
    {
        compareButton.onClick.AddListener(OnCompareButtonClick);
        LoadJsonData();
    }

    private void LoadJsonData()
    {
        // Load color.json
        string colorJsonText = Resources.Load<TextAsset>(colorJsonPath).text;
        colorNames = JsonUtility.FromJson<Colors>(colorJsonText).names;

        // Load Sort.json
        string sortJsonText = Resources.Load<TextAsset>(sortJsonPath).text;
        sortTexts = JsonUtility.FromJson<SortTexts>(sortJsonText).texts;

        // Load emblemAssets.json
        string emblemJsonText = Resources.Load<TextAsset>(emblemJsonPath).text;
        emblemNames = JsonUtility.FromJson<EmblemAssets>(emblemJsonText).names;
    }

    private void OnCompareButtonClick()
    {
        CompareAndPrintResult();
    }

    private void CompareAndPrintResult()
    {
        int index1 = GetColorNameIndex(element1.sprite.name);
        int index2 = GetSortTextIndex(element2.text);
        int index3 = GetEmblemNameIndex(element3.sprite.name);

        bool isSet = (index1 != -1 && index1 == index2 && index2 == index3);
        Debug.Log(isSet);
    }

    private int GetColorNameIndex(string spriteName)
    {
        string nameToCompare = spriteName.Substring(0, Mathf.Min(spriteName.Length, 3));

        for (int i = 0; i < colorNames.Count; i++)
        {
            if (colorNames[i].Contains(nameToCompare))
            {
                return i;
            }
        }

        return -1; // Not found
    }

    private int GetSortTextIndex(string text)
    {
        for (int i = 0; i < sortTexts.Count; i++)
        {
            if (sortTexts[i] == text)
            {
                return i;
            }
        }

        return -1; // Not found
    }

    private int GetEmblemNameIndex(string spriteName)
    {
        for (int i = 0; i < emblemNames.Count; i++)
        {
            if (emblemNames[i] == spriteName)
            {
                return i;
            }
        }

        return -1; // Not found
    }

    // Classes for JSON deserialization
    [System.Serializable]
    private class Colors
    {
        public List<string> names;
    }

    [System.Serializable]
    private class SortTexts
    {
        public List<string> texts;
    }

    [System.Serializable]
    private class EmblemAssets
    {
        public List<string> names;
    }
}
