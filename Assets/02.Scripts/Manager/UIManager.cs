using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Utils.EnumTypes;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance {  get { return instance; } }

    private Canvas mainCanvas;
    public TextMeshProUGUI timerText;
    private TextMeshProUGUI moneyText;
    private Slider reputationSlider;
    private Button homeButton;
    private AudioSource mainAudioSource;

    private GameObject exceptonObject;
    private Image exceptionImage;
    public Sprite[] exceptionSprite;

    private TextMeshProUGUI completeText;
    private TextMeshProUGUI customerText;
    private TextMeshProUGUI spotCompletetText;
    private TextMeshProUGUI SewingMachineText;

    private Canvas titleCanvas;
    private Button gameStartButton;
    private Button helpButton;
    private Button optionButton;
    private Button exitButton;

    private GameObject helpPhanel;
    private Button leftButton;
    private Button rightButton;
    private Button[] stepButtons;
    private Image helpImage;
    public Sprite[] helpSprites;
    private Image[] stepImages;
    private int currentStep = 0;

    private GameObject endingObject;
    private Animator endingAnimator;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        endingObject.SetActive(false);
        moneyText.text = string.Format("{0}", 0);
        reputationSlider.value = (50 / 100.0f);
        completeText.text = (GameManager.Instance.completeCount).ToString();
        customerText.text = (GameManager.Instance.customerCount).ToString();
        spotCompletetText.text = (GameManager.Instance.spotCompleteCount).ToString();
        SewingMachineText.text = (GameManager.Instance.sewingMachineCount).ToString();
    }

    public void TitleSceneInit()
    {
        titleCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        gameStartButton = titleCanvas.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        helpButton = titleCanvas.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        optionButton = titleCanvas.transform.GetChild(0).GetChild(3).GetComponent<Button>();
        exitButton = titleCanvas.transform.GetChild(0).GetChild(4).GetComponent<Button>();

        helpPhanel = titleCanvas.transform.GetChild(2)?.gameObject;
        leftButton = helpPhanel.transform.GetChild(0).GetChild(1).GetComponent<Button>();
        rightButton = helpPhanel.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        stepButtons = helpPhanel.transform.GetChild(0).GetChild(3).GetComponentsInChildren<Button>();
        helpImage = helpPhanel.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        stepImages = helpPhanel.transform.GetChild(0).GetChild(3).GetComponentsInChildren<Image>();
    }

    public void MainSceneInit()
    {
        mainCanvas = GameObject.Find("MainCanvas")?.GetComponent<Canvas>();
        mainAudioSource = mainCanvas.GetComponent<AudioSource>();
        timerText = mainCanvas.transform.GetChild(0).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
        moneyText = mainCanvas.transform.GetChild(0).GetChild(4).GetComponentInChildren<TextMeshProUGUI>();
        reputationSlider = mainCanvas.transform.GetChild(0).GetChild(2).GetComponent<Slider>();
        exceptonObject = mainCanvas.transform.GetChild(1).gameObject;
        exceptionImage = exceptonObject.GetComponent<Image>();

        completeText = mainCanvas.transform.GetChild(0).GetChild(5).GetComponentInChildren<TextMeshProUGUI>();
        customerText = mainCanvas.transform.GetChild(0).GetChild(6).GetComponentInChildren<TextMeshProUGUI>();
        spotCompletetText = mainCanvas.transform.GetChild(0).GetChild(7).GetComponentInChildren<TextMeshProUGUI>();
        SewingMachineText = mainCanvas.transform.GetChild(0).GetChild(8).GetComponentInChildren<TextMeshProUGUI>();
        homeButton = mainCanvas.transform.GetChild(0).GetChild(9).GetComponent<Button>();

        endingObject = mainCanvas.transform.GetChild(2).gameObject;
        endingAnimator = endingObject.transform.GetChild(0).GetComponent<Animator>();

        homeButton.onClick.AddListener(delegate {
            mainAudioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.Click];
            mainAudioSource.Play();
            SceneManager.LoadScene(0);
        });
        GameManager.Instance.ReputationHandler(0);
    }

    public void ButtonInit()
    {
        gameStartButton.onClick.AddListener(delegate
        {
            GameManager.Instance.GameStart();
        });
        helpButton.onClick.AddListener(delegate
        {
            TitleSceneInit();
            OnStep(0);
        });
        optionButton.onClick.AddListener(delegate
        {
            AudioManager.Instance.Init();
        });
        exitButton.onClick.AddListener(delegate
        {
            GameManager.Instance.GameExit();
        });

        leftButton.onClick.AddListener(delegate { OnNextStep(false); });
        rightButton.onClick.AddListener(delegate { OnNextStep(true); });
        stepButtons[0].onClick.AddListener(delegate { OnStep(0); });
        stepButtons[1].onClick.AddListener(delegate { OnStep(1); });
        stepButtons[2].onClick.AddListener(delegate { OnStep(2); });
        stepButtons[3].onClick.AddListener(delegate { OnStep(3); });
    }

    public void OnNextStep(bool _isRight)
    {
        if (_isRight)
        {
            if (currentStep >= 3)
                currentStep = 3;
            else
                currentStep += 1;
        }
        else
        {
            if (currentStep <= 0)
                currentStep = 0;
            else
                currentStep -= 1;
        }

        OnStep(currentStep);
    }

    public void OnStep(int _step)
    {
        for (int i = 0; i < stepImages.Length; i++)
        {
            if (i == _step)
            {
                currentStep = i;
                helpImage.sprite = helpSprites[i];
                stepImages[i].color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
            }
            else
            {
                stepImages[i].color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            }
        }
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
        endingAnimator.SetBool(_ending, true);

        yield return new WaitForSeconds(5.0f);

        endingObject.transform.GetChild(1).gameObject.SetActive(true);
        endingObject.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene(0); });
    }
}