using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance {  get { return instance; } }

    private Canvas canvas;
    public TextMeshProUGUI timerText;
    private TextMeshProUGUI moneyText;
    private Slider reputationSlider;

    private GameObject exceptonObject;
    private Image exceptionImage;
    public Sprite[] exceptionSprite;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        canvas = GameObject.Find("MainCanvas")?.GetComponent<Canvas>();
        timerText = canvas.transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        moneyText = canvas.transform.GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        reputationSlider = canvas.transform.GetChild(0).GetChild(2).GetComponent<Slider>();
        exceptonObject = canvas.transform.GetChild(1).gameObject;
        exceptionImage = exceptonObject.GetComponent<Image>();

        GameManager.Instance.ReputationHandler(0);
    }

    public void TimerHandler(float _time)
    {
        timerText.text = string.Format("{0:00}:{1:00}", (int)(_time / 60.0f), (int)(_time % 60.0f));
    }

    public void ChangeMoneyText(int _money)
    {
        moneyText.text = string.Format("{0:#,###}", _money);
    }

    public void ChangeReputationBar(int _reputation)
    {
        reputationSlider.value = (_reputation / 100.0f);
    }

    public IEnumerator OnException(int _index)
    {
        exceptionImage.sprite = exceptionSprite[_index];
        exceptonObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        exceptonObject.SetActive(false);
    }
}
