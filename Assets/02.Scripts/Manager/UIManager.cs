using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private TextMeshProUGUI completeText;
    private TextMeshProUGUI customerText;
    private TextMeshProUGUI spotCompletetText;
    private TextMeshProUGUI SewingMachineText;

    private GameObject endingObject;
    private TextMeshProUGUI endingText;

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

        completeText = canvas.transform.GetChild(0).GetChild(5).GetComponentInChildren<TextMeshProUGUI>();
        customerText = canvas.transform.GetChild(0).GetChild(6).GetComponentInChildren<TextMeshProUGUI>();
        spotCompletetText = canvas.transform.GetChild(0).GetChild(7).GetComponentInChildren<TextMeshProUGUI>();
        SewingMachineText = canvas.transform.GetChild(0).GetChild(8).GetComponentInChildren<TextMeshProUGUI>();

        endingObject = canvas.transform.GetChild(2).gameObject;
        endingText = endingObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

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

    public void ChangeCompleteText()
    {
        GameManager.Instance.completeCount += 1;
        completeText.text = (GameManager.Instance.completeCount).ToString();
    }

    public void ChangeCustomerText()
    {
        GameManager.Instance.customerCount += 1;
        customerText.text = (GameManager.Instance.customerCount).ToString();
    }

    public void ChangeSpotCompleteText()
    {
        GameManager.Instance.spotCompleteCount += 1;
        spotCompletetText.text = (GameManager.Instance.spotCompleteCount).ToString();
    }

    public void ChangeSewingMachineText()
    {
        GameManager.Instance.sewingMachineCount += 1;
        SewingMachineText.text = (GameManager.Instance.sewingMachineCount).ToString();
    }

    public IEnumerator OnException(int _index)
    {
        exceptionImage.sprite = exceptionSprite[_index];
        exceptonObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        exceptonObject.SetActive(false);
    }

    public IEnumerator OnEnding(string _ending)
    {
        endingObject.SetActive(true);
        endingText.text = _ending;

        yield return new WaitForSeconds(5.0f);

        endingObject.transform.GetChild(1).gameObject.SetActive(true);
        endingObject.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(0); });
    }
}
