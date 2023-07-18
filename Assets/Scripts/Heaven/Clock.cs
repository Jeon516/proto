using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour
{
    public Image circleImage;
    public GameObject ClockStick;
    public float fillDuration = 30f; // 게임 시간
    public GameObject GameResult;
    private float fillTimer = 0f;

    private void Update()
    {
        if (fillTimer < fillDuration && HeavenGameManager.Instance.Play)
        {
            ClockMove();
        }

        if(fillTimer >= fillDuration)
        {
            GameResult.SetActive(true);
        }
    }

    private void ClockMove()
    {
        fillTimer += Time.deltaTime;
        float fillAmount = fillTimer / fillDuration;
        circleImage.fillAmount = fillAmount;

        ClockStick.transform.Rotate(Vector3.forward, -360 / fillDuration * Time.deltaTime);
    } // 시계 움직이기
}
