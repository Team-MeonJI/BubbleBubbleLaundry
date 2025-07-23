using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    private int money = 0;
    private int reputation = 0;

    private float gamePlayTime = 600.0f;
    private float currentTime;

    private bool isGameOver = false;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

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
            currentTime = 0;
            isGameOver = true;
        }
    }

    // 초기화
    private void Init()
    {
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 65;

        currentTime = gamePlayTime;
    }

    // 게임 종료
    public void GameOver()
    {
        isGameOver = true;
    }
}