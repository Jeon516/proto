using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeskButton : MonoBehaviour
{
    private bool IsOpen=false; // 버튼 열리기 여부
    public bool IsExist; // 강아지가 존재 여부

    private Vector2 originalPosition;
    public RectTransform rectTransform;

    private void Start()
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnClick_Open()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            Vector2 newPosition = new Vector2(originalPosition.x -1300f, originalPosition.y);
            rectTransform.anchoredPosition = newPosition;
        } // 버튼 열리기
        else
        {
            IsOpen = false;
            Vector2 newPosition = new Vector2(originalPosition.x, originalPosition.y);
            rectTransform.anchoredPosition = newPosition;
        } // 버튼 닫기

    }
}
