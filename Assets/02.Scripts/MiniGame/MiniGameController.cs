using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour
{
    protected TextMeshProUGUI timerText;
    public GameObject resultPhanel;
    private Image resultImage;
    public Sprite[] resultSprites;

    protected float currentTime;
    protected const float miniGameTime = 30.0f;

    public bool isGameSuccess = false;
    public bool isGameOver = false;

    private void OnEnable()
    {
        Init();
    }

    // 초기화
    public virtual void Init()
    {
        timerText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        resultPhanel = transform.GetChild(1).gameObject;
        resultImage = resultPhanel.transform.GetChild(0).GetComponent<Image>();
    }

    // 미니게임 시작
    public virtual void MiniGameStart()
    {
        Debug.Log("::: MiniGame Start :::");
        Init();
        isGameOver = false;
        currentTime = miniGameTime;
        timerText.text = ((int)currentTime).ToString();
        transform.gameObject.SetActive(true);
    }

    // 미니게임 종료
    public virtual void MiniGameEnd()
    {
        isGameOver = true;
        resultPhanel.SetActive(true);
        resultImage.sprite = (isGameSuccess) ? resultSprites[0] : resultSprites[1];
    }

    // 게임 결과
    public virtual void MiniGameReward()
    {

    }
}