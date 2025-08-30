using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.EnumTypes;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    private int money = 0;
    public int reputation = 50;
    public int completeCount = 0;
    public int customerCount = 0;
    public int spotCompleteCount = 0;
    public int sewingMachineCount = 0;

    private float gamePlayTime = 600.0f;
    private float currentTime;

    public int reputeDecr = 3;
    public int reputeAdd = 2;

    public bool isGameOver = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Init();
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UIManager.Instance.TimerHandler(currentTime);
        }
        else
        {
            GameOver();
        }
    }

    // �ʱ�ȭ
    private void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 65;
    }

    // ��ȭ ����
    public void MoneyHandler(int _money)
    {
        if (money + _money <= 0)
            money = 0;
        else
            money += _money;

        UIManager.Instance.ChangeMoneyText(money);
    }

    // ���� ����
    public void ReputationHandler(int _reputation)
    {
        if (reputation + _reputation <= 0)
            reputation = 0;
        else if (reputation + _reputation >= 100)
            reputation = 100;
        else
            reputation += _reputation;

        UIManager.Instance.ChangeReputationBar(reputation);
    }

    // ���� ����
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    // ���� ����
    public void GameOver()
    {
        currentTime = 0;
        isGameOver = true;

        if(money >= 175000)
        {
            // ���� ����
            StartCoroutine(UIManager.Instance.OnEnding("IsHappy"));
            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.HappyEnding];
        }
        else if(money >= 100000 && money < 175000)
        {
            // �븻 ����
            StartCoroutine(UIManager.Instance.OnEnding("IsNormal"));
            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.NormalEnding];
        }
        else
        {
            // ���� ����
            StartCoroutine(UIManager.Instance.OnEnding("IsBad"));
            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.BadEnding];
        }

        AudioManager.Instance.audioSource.Play();
    }

    // ���� ������
    public void GameExit()
    {
        //Application.Quit();
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if(_scene.name == "TitleScene")
        {
            isGameOver = true;
            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.Title];
            AudioManager.Instance.audioSource.Play();
            UIManager.Instance.TitleSceneInit();
            UIManager.Instance.ButtonInit();
        }
        else if(_scene.name == "MainScene")
        {
            isGameOver = false;
            money = 0;
            reputation = 50;
            currentTime = gamePlayTime;
            completeCount = 0;
            customerCount = 0;
            spotCompleteCount = 0;
            sewingMachineCount = 0;

            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.Main];
            AudioManager.Instance.audioSource.Play();
            UIManager.Instance.MainSceneInit();
            UIManager.Instance.Init();
        }
    }
}