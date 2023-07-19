using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class EmblemLoader : MonoBehaviour
{
    public Image emblemImage;
    public string emblempng = "example"; // 이제 정적 변수가 아니라 인스턴스 변수로 사용
    private GameProcess gameProcess;

    private void Start()
    {
        gameProcess = FindObjectOfType<GameProcess>();
        GameProcess.OnButtonClicked += OnGameProcessButtonClicked;
        LoadEmblemImageFromJSON();
    }

    private void OnGameProcessButtonClicked()
    {
        LoadEmblemImageFromJSON();
    }

    private void LoadEmblemImageFromJSON()
    {
        /*string JSONFilePath = "JsonFiles/Game/Set";
        string JSONFullPath = Path.Combine("Assets/Resources", JSONFilePath);
        JSONFullPath += ".json";

        if (File.Exists(JSONFullPath))
        {
            string json = File.ReadAllText(JSONFullPath);
            SetDataList setDataList = JsonUtility.FromJson<SetDataList>(json);

            if (setDataList != null && setDataList.Sets.Count > 0)
            {
                string randomColor3 = gameProcess.previousRandomColor3; // Use the static emblemcolor variable

                List<SetData> matchingSets = setDataList.Sets.FindAll(setData => setData.Color == randomColor3);

                float matchingEmblemProbability = (float)gameProcess.SE / 100f;
                float nonMatchingEmblemProbability = (float)gameProcess.AE / 100f;

                bool useMatchingEmblem = Random.value < matchingEmblemProbability;

                string emblemValue;
                if (useMatchingEmblem && matchingSets.Count > 0)
                {
                    int randomIndex = Random.Range(0, matchingSets.Count);
                    emblemValue = matchingSets[randomIndex].emblemAssets;
                }
                else
                {
                    List<SetData> nonMatchingSets = setDataList.Sets.FindAll(setData => setData.Color != randomColor3);
                    int randomIndex = Random.Range(0, nonMatchingSets.Count);
                    emblemValue = nonMatchingSets[randomIndex].emblemAssets;
                }

                emblempng = emblemValue; // emblempng 값을 업데이트
                LoadEmblemImage(emblemValue);
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
            SetDataList setDataList = JsonUtility.FromJson<SetDataList>(jsonData);

            if (setDataList != null && setDataList.Sets.Count > 0)
            {
                string randomColor3 = gameProcess.previousRandomColor3; // Use the static emblemcolor variable

                List<SetData> matchingSets = setDataList.Sets.FindAll(setData => setData.Color == randomColor3);

                float matchingEmblemProbability = (float)gameProcess.SE / 100f;
                float nonMatchingEmblemProbability = (float)gameProcess.AE / 100f;

                bool useMatchingEmblem = Random.value < matchingEmblemProbability;

                string emblemValue;
                if (useMatchingEmblem && matchingSets.Count > 0)
                {
                    int randomIndex = Random.Range(0, matchingSets.Count);
                    emblemValue = matchingSets[randomIndex].emblemAssets;
                }
                else
                {
                    List<SetData> nonMatchingSets = setDataList.Sets.FindAll(setData => setData.Color != randomColor3);
                    int randomIndex = Random.Range(0, nonMatchingSets.Count);
                    emblemValue = nonMatchingSets[randomIndex].emblemAssets;
                }

                emblempng = emblemValue; // emblempng 값을 업데이트
                LoadEmblemImage(emblemValue);
            }
            else
            {
                Debug.LogError("셋에 데이터가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("JSON file not found.");
        }
    }

    private void LoadEmblemImage(string emblemValue)
    {
        string imagePath = Path.Combine("Download Assets", "Fantasy Emblem3(living) Set Pack", "FantasyEmblem3_128_W", emblemValue);

        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            Sprite emblemSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            emblemImage.sprite = emblemSprite;
        }
        else
        {
            Debug.LogError("해당 주소에 대한 엠블럼 이미지를 찾을 수 없습니다: " + emblemValue);
        }
    }


    private Texture2D LoadTextureFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            return texture;
        }
        else
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + filePath);
            return null;
        }
    }
}
