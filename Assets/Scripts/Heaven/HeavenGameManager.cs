using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeavenGameManager : MonoBehaviour
{
    public int Money;
    public int Day;
    public Text DayText;
    public GameObject GameStart;
    public bool Play = false;

    public static HeavenGameManager Instance { get; private set; } = null;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Day", 1) == 0)
            Day = 1;
        else
            Day = PlayerPrefs.GetInt("Day", 1);
        PlayerPrefs.SetInt("Day", Day);
        Instance = this;
    }
    private void Start()
    {
        DayText.text = Day.ToString() + "일 째";
        if (Day >= 2)
            GameStart.SetActive(true);
        else
            GameStart.SetActive(false);
    }
    public void OnClick_LeftStation()
    {
        DogWait.Instance.ChangeImage();
        Debug.Log("Left");
    }
    public void OnClick_RightStation()
    {
        DogWait.Instance.ChangeImage();
        Debug.Log("Right");
    }
    public void OnClick_CancelStation()
    {
        DogWait.Instance.ChangeImage();
        Debug.Log("Cancel");
    }

    public void OnClick_GameStart()
    {
        GameStart.SetActive(false);
        Play = true;
    } // 게임 스타트

    public void OnClick_NextScene()
    {
        if(Day!=1)
            SceneManager.LoadScene("Upbringing");
        else
            SceneManager.LoadScene("Tutorial");
    }
}
