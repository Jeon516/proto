using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public Text correctRateText; // 정답률을 표시할 텍스트
    public Text wrongRateText; // 오답률을 표시할 텍스트
    public Text correctPerMinText; // 분당 정답수를 표시할 텍스트
    public Text currentText;
    public Text AvgComboText;
    private GameProcess gameProcess; // GameProcess 스크립트의 인스턴스를 참조하는 변수
    public GameObject Return; // 리턴버튼
    public GameObject Restart; // 스타트 버튼
    public GameObject ResultPref; // 결과 화면
    public GameObject EndText; // 끝 텍스트


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
        if (gameProcess.dayValue == 20)
        {
            Restart.SetActive(false);
            EndText.SetActive(true);
        }
        else if (gameProcess.dayValue < 20)
        {
            Restart.SetActive(true);
        } // 날짜가 20일인지 아닌지 여부

        int correctCount = gameProcess.correct;
        int wrongCount = gameProcess.wrongCount;
        int totalCount = gameProcess.totalCount;
        int Current = gameProcess.ingamegold;
        int combomax = gameProcess.Combomax;
        int combonum = gameProcess.ComboCount;

        float correctRate = (float)correctCount / totalCount * 100f;
        float wrongRate = (float)wrongCount / totalCount * 100f;
        float correctPerMin = (float)correctCount / 90;
        float AvgCombo = (float)combomax/combonum;

        correctRateText.text = "정답률: " + correctRate.ToString("F1") + "%";
        wrongRateText.text = "오답률: " + wrongRate.ToString("F1") + "%";
        correctPerMinText.text = "초당 정답수: " + correctPerMin.ToString("F1");
        currentText.text = "획득 재화량: " + Current.ToString();
        AvgComboText.text = "평균 콤보 수: " + AvgCombo.ToString("F1");

    }
    public void OnClick_Restart()
    {
        gameProcess.combo=0;
        gameProcess.ingamegold=0;
        gameProcess.ComboCount=0;
        gameProcess.Combomax=0;
        gameProcess.correct=0;
        gameProcess.wrongCount=0;
        gameProcess.totalCount=0;
        TimerController.Instance.ResetGame();
        gameProcess.dayValue++;
        Restart.SetActive(true);
        ResultPref.SetActive(false);
    }
}
