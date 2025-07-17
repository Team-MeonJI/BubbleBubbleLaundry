using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; }}

    private int money = 0;
    private int reputation = 0;

    private float currentTime = 0.0f;
    private float gamePlayTime = 600.0f;

    private bool isGameOver = false;

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
        Screen.SetResolution(1920, 1080, true);
        Application.targetFrameRate = 65;
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}