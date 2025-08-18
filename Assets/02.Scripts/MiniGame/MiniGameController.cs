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
    protected const float miniGameTime = 17.0f;

    public bool isGameSuccess = false;
    public bool isGameOver = false;

    private void OnEnable()
    {
        Init();
    }

    // �ʱ�ȭ
    public virtual void Init()
    {
        timerText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        resultPhanel = transform.GetChild(1).gameObject;
        resultImage = resultPhanel.transform.GetChild(0).GetComponent<Image>();
    }

    // �̴ϰ��� ����
    public virtual void MiniGameStart()
    {
        Debug.Log("::: MiniGame Start :::");
        Init();
        isGameOver = false;
        currentTime = miniGameTime;
        timerText.text = ((int)currentTime).ToString();
        transform.gameObject.SetActive(true);
    }

    // �̴ϰ��� ����
    public virtual void MiniGameOver()
    {
        isGameOver = true;
        resultPhanel.SetActive(true);
        resultImage.sprite = (isGameSuccess) ? resultSprites[0] : resultSprites[1];
    }

    // ���� ���
    public virtual void MiniGameReward()
    {

    }
}