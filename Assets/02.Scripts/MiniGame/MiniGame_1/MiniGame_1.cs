using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Utils.EnumTypes;
using System.Collections;

public class MiniGame_1 : MiniGameController
{
    private AudioSource audioSource;
    private GameObject clothes;
    public Sprite[] clotheSprites;

    public List<GameObject> spots;
    public GameObject spotPrefab;

    private float randomRangeX = 100.0f;
    private float randomRangeY = 160.0f;

    private int spotCount = 0;
    private const int minSpotCount = 3;
    private const int maxSpotCount = 7;
    private const int reward = 100;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clothes = transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        spotCount = Random.Range(minSpotCount, maxSpotCount);
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerText.text = ((int)currentTime).ToString();

            if (spots.Count <= 0)
            {
                isGameSuccess = true;
                StartCoroutine(GameEnd());
                //MiniGameOver();
            }
        }
        else
        {
            isGameSuccess = false;
            StartCoroutine(GameEnd());
            //MiniGameOver();
        }
    }

    public override void Init()
    {
        base.Init();

        isGameOver = false;
        isGameSuccess = false;
        currentTime = miniGameTime;
        resultPhanel.SetActive(false);

        clothes.GetComponent<Image>().sprite = clotheSprites[Random.Range(0, clotheSprites.Length)];

        for (int i = 0; i < spotCount; i++)
        {
            spots.Add(Instantiate(spotPrefab, clothes.transform));
            spots[i].transform.position = SetRandomPosition();
        }
    }

    public override void MiniGameStart()
    {
        base.MiniGameStart();
    }

    public override void MiniGameOver()
    {
        base.MiniGameOver();

        if (isGameSuccess)
            audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.MiniGame_Clear];
        else
            audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.MiniGame_Over];

        audioSource.Play();
    }

    public override void MiniGameReward()
    {
        base.MiniGameReward();

        if (isGameSuccess)
        {
            UIManager.Instance.ChangeSpotCompleteText();
            GameManager.Instance.ReputationHandler(5);
            GameManager.Instance.MoneyHandler(reward * (int)currentTime);
        }
        else
        {
            GameManager.Instance.ReputationHandler(-10);
        }

        transform.gameObject.SetActive(false);
        MiniGameManager.Instance.OnMiniGameEnd(isGameSuccess);
    }

    public IEnumerator GameEnd()
    {
        MiniGameOver();
        yield return new WaitForSeconds(audioSource.clip.length);
        MiniGameReward();
    }

    // ¾ó·è ·£´ý À§Ä¡ ¼³Á¤
    public Vector2 SetRandomPosition()
    {
        Vector2 _anchorPosition = clothes.GetComponent<RectTransform>().position;
        Vector2 _randomOffest = new Vector2(Random.Range(-randomRangeX, randomRangeX), Random.Range(-randomRangeY, randomRangeY));
        return _anchorPosition + _randomOffest;
    }
}