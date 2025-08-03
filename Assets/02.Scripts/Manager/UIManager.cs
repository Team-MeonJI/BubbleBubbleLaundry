using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance {  get { return instance; } }

    private Canvas canvas;
    public TextMeshProUGUI timerText;
    private TextMeshProUGUI moneyText;
    private GameObject exceptonObject;
    private TextMeshProUGUI exceptionText;

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
        timerText = canvas.transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        moneyText = canvas.transform.GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        exceptonObject = canvas.transform.GetChild(1).gameObject;
        exceptionText = exceptonObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
    }

    public void TimerHandler(float _time)
    {
        timerText.text = string.Format("{0:00}:{1:00}", (int)(_time / 60.0f), (int)(_time % 60.0f));
    }

    public void MoneyHandler(int _money)
    {
        moneyText.text = _money.ToString();
    }

    public IEnumerator OnException(string _text)
    {
        exceptionText.text = _text;
        exceptonObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        exceptonObject.SetActive(false);
    }
}
