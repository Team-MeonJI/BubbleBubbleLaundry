using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance {  get { return instance; } }

    private Canvas canvas;
    public TextMeshProUGUI timerText;
    private TextMeshProUGUI moneyText;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        Init();
    }

    private void Init()
    {
        canvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        timerText = canvas.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        moneyText = canvas.transform.GetChild(0).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void TimerHandler(float _time)
    {
        timerText.text = string.Format("{0:00}:{1:00}", (int)(_time / 60.0f), (int)(_time % 60.0f));
    }

    public void MoneyHandler(int _money)
    {

    }
}
