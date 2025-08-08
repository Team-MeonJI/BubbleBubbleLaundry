using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.EnumTypes;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    private int money = 0;
    private int reputation = 50;
    public int completeCount = 0;
    public int customerCount = 0;
    public int spotCompleteCount = 0;
    public int sewingMachineCount = 0;

    private float gamePlayTime = 600.0f;
    private float currentTime;

    public bool isGameOver = true;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        Init();
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

    // 초기화
    private void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 65;

        currentTime = gamePlayTime;
    }

    // 재화 관리
    public void MoneyHandler(int _money)
    {
        if (money + _money <= 0)
            money = 0;
        else
            money += _money;

        UIManager.Instance.ChangeMoneyText(money);
    }

    // 평판 관리
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

    // 게임 시작
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    // 게임 종료
    public void GameOver()
    {
        currentTime = 0;
        isGameOver = true;

        if(money >= 15000)
        {
            // 해피 엔딩
            StartCoroutine(UIManager.Instance.OnEnding("해피 엔딩"));
            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.HappyEnding];
        }
        else if(money >= 5000 && money < 15000)
        {
            // 노말 엔딩
            StartCoroutine(UIManager.Instance.OnEnding("노말 엔딩"));
            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.NormalEnding];
        }
        else
        {
            // 베드 엔딩
            StartCoroutine(UIManager.Instance.OnEnding("베드 엔딩"));
            AudioManager.Instance.audioSource.clip = AudioManager.Instance.bgmClips[(int)BGMType.BadEnding];
        }
    }

    // 게임 나가기
    public void GameExit()
    {
        Application.Quit();
    }

    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if(_scene.name == "MainScene")
        {
            UIManager.Instance.Init();
            isGameOver = false;
        }
    }
}