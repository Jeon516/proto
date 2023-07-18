using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text correctRateText; // 정답률을 표시할 텍스트
    public Text wrongRateText; // 오답률을 표시할 텍스트
    public Text correctPerMinText; // 분당 정답수를 표시할 텍스트
    private GameProcess gameProcess; // GameProcess 스크립트의 인스턴스를 참조하는 변수

    private void Start()
    {
        gameProcess = GetComponent<GameProcess>(); // GameProcess 스크립트가 붙어 있는 게임 오브젝트에서 컴포넌트를 직접 참조

        if (gameProcess == null)
        {
            Debug.LogError("GameProcess 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (gameProcess == null)
        {
            Debug.LogError("GameProcess 인스턴스가 할당되지 않았습니다.");
            return; // gameProcess가 null이면 업데이트를 종료
        }

        int correctCount = gameProcess.CorrectCount;
        int wrongCount = gameProcess.WrongCount;
        int totalCount = gameProcess.TotalCount;

        float correctRate = (float)correctCount / totalCount * 100f;
        float wrongRate = (float)wrongCount / totalCount * 100f;
        float correctPerMin = (float)correctCount / 60;

        correctRateText.text = "정답률: " + correctRate.ToString("F1") + "%";
        wrongRateText.text = "오답률: " + wrongRate.ToString("F1") + "%";
        correctPerMinText.text = "초당 정답수: " + correctPerMin.ToString("F1");
    }
}
