using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class GameProcess : MonoBehaviour
{
    public Button buttonColor1;
    public Button buttonColor2;
    public Button buttonChangeColors;
    public Button buttonDifferentColor;
    public Image TrainLeft;
    public Image TrainRight;
    public Image Ticket;
    public Text DayText; // 날짜 텍스트
    public static string emblemcolor;
    private int correct = 0;
    private int combo = 0;
    private int wrongCount = 0;
    private int totalCount = 0;
    private bool isFirstRound = true;
    private TimerController timerController;

    public int CorrectCount
    {
        get { return correct; }
    }

    public int WrongCount
    {
        get { return wrongCount; }
    }

    public int TotalCount
    {
        get { return totalCount; }
    }

    [SerializeField] public int dayValue = 1;

    private int TL;
    private int TR;
    private int R;
    public int SN;
    public int AN;
    public int SE;
    public int AE;

    private List<Sprite> ticketSprites = new List<Sprite>();
    private List<string> availableColors = new List<string>();
    public delegate void ButtonClickedDelegate();
    public static event ButtonClickedDelegate OnButtonClicked;
    private List<SetData> setDataList = new List<SetData>();
    private TextLoader textLoader;
    private EmblemLoader emblemLoader;

    private string randomColor1;
    private string randomColor2;
    public string randomColor3;
    private List<string> previousRandomColors = new List<string>();

    public string previousRandomColor3; // 이전 값을 저장할 변수 추가

    private float timeLeft; // timeLeft 변수 추가

    private void Awake()
    {
        dayValue = 1;
    }
    private void Update()
    {
        DayText.text = "Day : " + dayValue;
    }
    private void Start()
    {
        buttonChangeColors.onClick.AddListener(OnChangeColorsButtonClick);
        textLoader = GetComponent<TextLoader>();
        emblemLoader = GetComponent<EmblemLoader>();
        timerController = FindObjectOfType<TimerController>();
        timeLeft = timerController.timeLeft;

        TL = 40;
        TR = 40;
        R = 20;
        SN = 80;
        AN = 20;
        SE = 80;
        AE = 20;

        if (dayValue >= 1 && dayValue <= 5)
        {
            TL = 40;
            TR = 40;
            R = 20;
            SN = 80;
            AN = 20;
            SE = 80;
            AE = 20;
        }
        else if (dayValue >= 6 && dayValue <= 15)
        {
            TL--;
            TR--;
            R += 2;
            SN -= 2;
            AN += 2;
            SE -= 2;
            AE += 2;
        }
        else if (dayValue >= 16 && dayValue <= 20)
        {
            TL = 30;
            TR = 30;
            R = 40;
            SN = 60;
            AN = 40;
            SE = 60;
            AE = 40;
        }

        LoadSetDataList();
        LoadTicketImages();

        buttonColor1.onClick.AddListener(OnColor1ButtonClick);
        buttonColor2.onClick.AddListener(OnColor2ButtonClick);
        buttonDifferentColor.onClick.AddListener(OnDifferentColorButtonClick);

        SetRandomColors();
        UpdateGameRound();
        timeLeft = timerController.timeLeft; // timeLeft 변수 초기화
    }

    private void LoadSetDataList()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JsonFiles/Game/Set");
        if (jsonFile != null)
        {
            string jsonData = jsonFile.text;
            SetDataList setDataList = JsonUtility.FromJson<SetDataList>(jsonData);

            if (setDataList != null && setDataList.Sets.Count > 0)
            {
                foreach (SetData setData in setDataList.Sets)
                {
                    availableColors.Add(setData.Color);
                }
            }
            else
            {
                Debug.LogError("Invalid JSON data or empty sets in the JSON file.");
            }
        }
        else
        {
            Debug.LogError("JSON file not found.");
        }
    }

    private void LoadTicketImages()
    {
        string folderPath = "Image/Heaven/tickets/";

        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(folderPath);
        ticketSprites.Clear();
        ticketSprites.AddRange(loadedSprites);

        if (ticketSprites.Count == 0)
        {
            Debug.LogError("Failed to load ticket images. Check the folder path: " + folderPath);
        }
    }

    private void SetRandomColors()
    {
        previousRandomColor3 = randomColor3;

        if (randomColor1 == null || randomColor2 == null)
        {
            int randomIndex1 = Random.Range(0, availableColors.Count);
            int randomIndex2 = Random.Range(0, availableColors.Count);

            while (randomIndex1 == randomIndex2)
            {
                randomIndex2 = Random.Range(0, availableColors.Count);
            }

            randomColor1 = availableColors[randomIndex1];
            randomColor2 = availableColors[randomIndex2];
            isFirstRound = false;
        }

        int totalProbability = TL + TR + R;
        int tlProbability = TL;
        int trProbability = TR;
        int rProbability = R;

        int randomProbability = Random.Range(1, totalProbability + 1);

        if (randomProbability <= tlProbability)
        {
            randomColor3 = randomColor1;
        }
        else if (randomProbability <= tlProbability + trProbability)
        {
            randomColor3 = randomColor2;
        }
        else
        {
            List<string> remainingColors = availableColors.FindAll(color => color != randomColor1 && color != randomColor2);
            int randomIndex = Random.Range(0, remainingColors.Count);
            randomColor3 = remainingColors[randomIndex];
        }

        if (previousRandomColors.Count > 2)
        {
            previousRandomColors.RemoveAt(0);
        }

        previousRandomColors.Add(randomColor3);
    }

    private Sprite GetTrainSprite(string color, bool isLeftSide)
    {
        string folderPath = isLeftSide ? "Image/Heaven/Train(Size)/LeftSide/" : "Image/Heaven/Train(Size)/RightSide/";
        string imagePath = folderPath + color;

        Sprite sprite = Resources.Load<Sprite>(imagePath);
        if (sprite == null)
        {
            Debug.LogError("Failed to find train image for the color: " + color);
        }

        return sprite;
    }

    private Sprite GetTicketSprite(string color)
    {
        foreach (Sprite ticketSprite in ticketSprites)
        {
            if (ticketSprite != null && ticketSprite.name != null && ticketSprite.name.Contains(color))
            {
                return ticketSprite;
            }
        }

        Debug.LogError("Failed to find ticket image for the color: " + color);
        return null;
    }

    private void UpdateGameRound()
    {
        emblemcolor = randomColor3;
        Ticket.sprite = GetTicketSprite(randomColor3);

        List<SetData> matchingSets = setDataList.FindAll(setData => setData.Color == randomColor3 && setData.emblemAssets == emblemLoader.emblempng && setData.Sort == textLoader.TextValue);

        if (matchingSets.Count > 0)
        {
            correct++;
            Debug.Log("Correct! Correct count: " + correct);
        }
        else
        {
            Debug.Log("Wrong! Correct count: " + correct);
        }
        Debug.Log("1,2:" + randomColor1 + randomColor2);
        Debug.Log("RandomColor3: " + randomColor3);
    }


    private void OnColor1ButtonClick()
    {
        if (randomColor1 == randomColor3)
        {
            if (previousRandomColor3 == "ticket to heaven 3" && textLoader.TextValue == "바다의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_03")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 4" && textLoader.TextValue == "행운의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_04")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 6" && textLoader.TextValue == "간식의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_06")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 7" && textLoader.TextValue == "자연의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_07")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 8" && textLoader.TextValue == "과일의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_08")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 9" && textLoader.TextValue == "마음의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_09")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 13" && textLoader.TextValue == "겨울의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_13")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 15" && textLoader.TextValue == "별빛의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_15")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 21" && textLoader.TextValue == "모험의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_21")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 35" && textLoader.TextValue == "도서의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_35")
            {
                combo++;
                correct++;
            }
            else
            {
                combo = 0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
        }
        else
        {
            combo = 0;
            wrongCount++;
            timerController.timeLeft -= 2f;
        }

        totalCount++;
        SetRandomColors();
        TrainLeft.sprite = GetTrainSprite(randomColor1, true);
        UpdateGameRound();
        Debug.Log("Combo: " + combo + " Correct: " + correct + " Wrong: " + wrongCount + " Total: " + totalCount);
        Debug.Log("Pre: " + previousRandomColor3 + " RandomColor3: " + randomColor3);

        if (OnButtonClicked != null)
        {
            OnButtonClicked();
        }
    }

    private void OnColor2ButtonClick()
    {
        if (randomColor2 == randomColor3)
        {
            if (previousRandomColor3 == "ticket to heaven 3" && textLoader.TextValue == "바다의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_03")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 4" && textLoader.TextValue == "행운의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_04")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 6" && textLoader.TextValue == "간식의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_06")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 7" && textLoader.TextValue == "자연의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_07")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 8" && textLoader.TextValue == "과일의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_08")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 9" && textLoader.TextValue == "마음의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_09")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 13" && textLoader.TextValue == "겨울의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_13")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 15" && textLoader.TextValue == "별빛의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_15")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 21" && textLoader.TextValue == "모험의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_21")
            {
                combo++;
                correct++;
            }
            else if (previousRandomColor3 == "ticket to heaven 35" && textLoader.TextValue == "도서의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_35")
            {
                combo++;
                correct++;
            }
            else
            {
                combo = 0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
        }
        else
        {
            combo = 0;
            wrongCount++;
            timerController.timeLeft -= 2f;
        }

        totalCount++;
        SetRandomColors();
        TrainRight.sprite = GetTrainSprite(randomColor2, false);
        UpdateGameRound();
        Debug.Log("Combo: " + combo + " Correct: " + correct + " Wrong: " + wrongCount + " Total: " + totalCount);
        Debug.Log("Pre: " + previousRandomColor3 + " RandomColor3: " + randomColor3);

        if (OnButtonClicked != null)
        {
            OnButtonClicked();
        }
    }

    private void OnChangeColorsButtonClick()
    {
        SetRandomColors();
        TrainLeft.sprite = GetTrainSprite(randomColor1, true);
        TrainRight.sprite = GetTrainSprite(randomColor2, false);
        UpdateGameRound();
        Debug.Log("Combo: " + combo + " Correct: " + correct + " Wrong: " + wrongCount + " Total: " + totalCount);
        Debug.Log("Pre: " + previousRandomColor3 + " RandomColor3: " + randomColor3);

        if (OnButtonClicked != null)
        {
            OnButtonClicked();
        }
    }

    private void OnDifferentColorButtonClick()
    {
        if (randomColor2 == randomColor3)
        {
            if (randomColor3 == "ticket to heaven 3" && textLoader.TextValue == "바다의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_03")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 4" && textLoader.TextValue == "행운의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_04")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 6" && textLoader.TextValue == "간식의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_06")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 7" && textLoader.TextValue == "자연의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_07")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 8" && textLoader.TextValue == "과일의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_08")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 9" && textLoader.TextValue == "마음의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_09")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 13" && textLoader.TextValue == "겨울의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_13")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 15" && textLoader.TextValue == "별빛의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_15")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 21" && textLoader.TextValue == "모험의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_21")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 35" && textLoader.TextValue == "도서의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_35")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else
            {
                combo++;
                correct++;
            }
        }
        else if (randomColor1 == randomColor3)
        {
            if (randomColor3 == "ticket to heaven 3" && textLoader.TextValue == "바다의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_03")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 4" && textLoader.TextValue == "행운의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_04")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 6" && textLoader.TextValue == "간식의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_06")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 7" && textLoader.TextValue == "자연의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_07")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 8" && textLoader.TextValue == "과일의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_08")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 9" && textLoader.TextValue == "마음의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_09")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 13" && textLoader.TextValue == "겨울의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_13")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 15" && textLoader.TextValue == "별빛의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_15")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 21" && textLoader.TextValue == "모험의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_21")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else if (randomColor3 == "ticket to heaven 35" && textLoader.TextValue == "도서의" && emblemLoader.emblempng == "FantasyEmblem3_256_B_35")
            {
                combo=0;
                wrongCount++;
                timerController.timeLeft -= 2f;
            }
            else
            {
                combo++;
                correct++;
            }
        }
        else
        {
            combo++;
            correct++;
        }

        totalCount++;
        SetRandomColors();
        TrainLeft.sprite = GetTrainSprite(randomColor1, true);
        TrainRight.sprite = GetTrainSprite(randomColor2, false);
        UpdateGameRound();
        Debug.Log("Combo: " + combo + " Correct: " + correct + " Wrong: " + wrongCount + " Total: " + totalCount);
        Debug.Log("emblem" + emblemLoader.emblempng + "text" + textLoader.TextValue);
        Debug.Log("Pre: " + previousRandomColor3 + "RandomColor3: " + randomColor3);

        if (OnButtonClicked != null)
        {
            OnButtonClicked();
        }
    }
}

[System.Serializable]
public class SetDataList
{
    public List<SetData> Sets;
}

[System.Serializable]
public class SetData
{
    public string Color;
    public string emblemAssets;
    public string Sort;
}