using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float gameDuration = 180f; 
    public Text timerText; 
    public Button exitButton; 
    public GameObject startButtonPrefab; 
    public GameObject newPrefab;
    public Button activatePrefabButton; 
    public Button[] interactiveButtons;

    public float timeLeft;
    private bool allowInteraction = false; 

    private void Start()
    {
        Time.timeScale = 1f;

       
        if (exitButton != null)
        {
            exitButton.interactable = false;
        }

        
        SetInteractiveButtonsInteractable(false);

        
        if (startButtonPrefab != null)
        {
            Button startButton = startButtonPrefab.GetComponentInChildren<Button>();
            if (startButton != null)
            {
                startButton.onClick.AddListener(StartGame);
            }
        }

        
        if (activatePrefabButton != null)
        {
            activatePrefabButton.onClick.AddListener(ActivateNewPrefab);
        }
    }

    private void Update()
    {
        if (allowInteraction)
        {
            timeLeft -= Time.deltaTime; 

            if (timeLeft <= 0f)
            {
                EndGame(); 
            }
        }

        UpdateTimerText();
        CheckInteractiveButtons();
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            if (allowInteraction)
            {
                timerText.text = "남은 시간: " + Mathf.CeilToInt(timeLeft).ToString();
            }
            else
            {
                timerText.text = "";
            }
        }
    }

    private void EndGame()
    {

        allowInteraction = false; 


        if (exitButton != null)
        {
            exitButton.interactable = true;
        }


        SetInteractiveButtonsInteractable(false);
    }

    private void SetInteractiveButtonsInteractable(bool interactable)
    {
        foreach (Button button in interactiveButtons)
        {
            if (button != null)
            {
                button.interactable = interactable;
            }
        }
    }

    private void CheckInteractiveButtons()
    {
        bool shouldEnableButtons = allowInteraction && timeLeft > 0f;

        foreach (Button button in interactiveButtons)
        {
            if (button != null)
            {
                button.interactable = shouldEnableButtons;
            }
        }
    }

    public void StartGame()
    {
        allowInteraction = true;
        timeLeft = gameDuration;


        if (startButtonPrefab != null)
        {
            startButtonPrefab.SetActive(false);
        }


        if (exitButton != null)
        {
            exitButton.interactable = false;
        }


        SetInteractiveButtonsInteractable(true);
    }

    public void ActivateNewPrefab()
    {
        if (newPrefab != null)
        {
            newPrefab.SetActive(true);
        }
    }
}
