using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TextLoader : MonoBehaviour
{
    public Text displayText;
    private string textValue = "example_text_value";
    public string TextValue
    {
        get { return textValue; }
    }

    private GameProcess gameProcess; // Reference to the GameProcess script
    private List<SetData> setDataList; // List to store the loaded set data

    private void Start()
    {
        gameProcess = FindObjectOfType<GameProcess>(); // Assign the reference to the GameProcess script
        GameProcess.OnButtonClicked += OnGameProcessButtonClicked;
        LoadTextFromJSON();
    }

    private void OnGameProcessButtonClicked()
    {
        LoadTextFromJSON();
    }

    private void LoadTextFromJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonFiles/Game/Set");
        if (jsonFile != null)
        {
            string jsonData = jsonFile.text;
            SetDataList loadedData = JsonUtility.FromJson<SetDataList>(jsonData);

            if (loadedData != null && loadedData.Sets.Count > 0)
            {
                setDataList = loadedData.Sets;
                string rColor3 = gameProcess.GetRandomColor3();

                // Set.json파일에서 Color == Randomcolor3인 그룹을 확인
                List<SetData> matchingSets = setDataList.FindAll(setData => setData.Color == rColor3);

                // Probability for assigning Sort value from the same group (SE)
                float SEProbability = (float)gameProcess.SE / 100f;

                // Probability for assigning Sort value from other non-matching groups (AE)
                float AEProbability = (float)gameProcess.AE / 100f;

                string textValue;
                if (matchingSets.Count > 0 && Random.value < SEProbability)
                {
                    // Get wave text or other matching text
                    int randomIndex = Random.Range(0, matchingSets.Count);
                    textValue = matchingSets[randomIndex].Sort;
                }
                else
                {
                    // Get other non-matching text
                    List<SetData> nonMatchingSets = setDataList.FindAll(setData => setData.Color != rColor3);
                    int randomIndex = Random.Range(0, nonMatchingSets.Count);
                    textValue = nonMatchingSets[randomIndex].Sort;
                }

                // Update textValue with the new value
                this.textValue = textValue;

                // Set the text to displayText.text
                displayText.text = textValue;
            }
        }
        else
        {
            Debug.LogError("JSON file not found.");
        }
    }
}
