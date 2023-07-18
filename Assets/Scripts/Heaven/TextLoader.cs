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
        /*string JSONFilePath = "JsonFiles/Game/Set";
        string JSONFullPath = Path.Combine("Assets/Resources", JSONFilePath);
        JSONFullPath += ".json";

        if (File.Exists(JSONFullPath))
        {
            string json = File.ReadAllText(JSONFullPath);
            SetDataList loadedData = JsonUtility.FromJson<SetDataList>(json);
            if (loadedData != null && loadedData.Sets.Count > 0)
            {
                setDataList = loadedData.Sets;
                string rColor3 = gameProcess.previousRandomColor3;

                float waveProbability = 0f;
                float otherProbability = 0f;

                List<SetData> matchingSets = setDataList.FindAll(setData => setData.Color == rColor3);

                if (matchingSets.Count > 0)
                {
                    waveProbability = (float)gameProcess.SE / 100f;
                    otherProbability = (float)gameProcess.AE / (matchingSets.Count + 1);
                }
                else
                {
                    waveProbability = (float)gameProcess.AE / 100f;
                    otherProbability = waveProbability;
                }

                bool useWaveText = Random.value < waveProbability;

                string textValue;
                if (useWaveText)
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
            else
            {
                Debug.LogError("셋에 데이터가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + JSONFullPath);
        }*/
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonFiles/Game/Set");
        if (jsonFile != null)
        {
            string jsonData = jsonFile.text;
            SetDataList loadedData = JsonUtility.FromJson<SetDataList>(jsonData);

            if (loadedData != null && loadedData.Sets.Count > 0)
            {
                setDataList = loadedData.Sets;
                string rColor3 = gameProcess.previousRandomColor3;

                float waveProbability = 0f;
                float otherProbability = 0f;

                List<SetData> matchingSets = setDataList.FindAll(setData => setData.Color == rColor3);

                if (matchingSets.Count > 0)
                {
                    waveProbability = (float)gameProcess.SE / 100f;
                    otherProbability = (float)gameProcess.AE / (matchingSets.Count + 1);
                }
                else
                {
                    waveProbability = (float)gameProcess.AE / 100f;
                    otherProbability = waveProbability;
                }

                bool useWaveText = Random.value < waveProbability;

                string textValue;
                if (useWaveText)
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
