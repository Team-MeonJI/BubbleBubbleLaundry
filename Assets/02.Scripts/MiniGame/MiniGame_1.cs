using System.Collections.Generic;
using UnityEngine;

public class MiniGame_1 : MiniGameController
{
    private GameObject clothes;

    public List<GameObject> spots;
    public GameObject spotPrefab;

    private float randomRangeX = 400.0f;
    private float randomRangeY = 250.0f;

    private int spotCount = 0;
    private const int minSpotCount = 3;
    private const int maxSpotCount = 7;
    private const int reward = 100;

    private void Awake()
    {
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
                MiniGameEnd();
            }
        }
        else
        {
            isGameSuccess = false;
            MiniGameEnd();
        }
    }

    public override void Init()
    {
        base.Init();

        isGameOver = false;
        isGameSuccess = false;
        currentTime = miniGameTime;
        resultPhanel.SetActive(false);

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

    public override void MiniGameEnd()
    {
        base.MiniGameEnd();
    }

    public override void MiniGameReward()
    {
        base.MiniGameReward();

        if (isGameSuccess)
        {
            GameManager.Instance.ReputationHandler(5);
            GameManager.Instance.MoneyHandler(reward * (int)currentTime);
        }
        else
            GameManager.Instance.ReputationHandler(-10);

        transform.gameObject.SetActive(false);
        MiniGameManager.Instance.OnMiniGameEnd(isGameSuccess);
    }

    // ¾ó·è ·£´ý À§Ä¡ ¼³Á¤
    public Vector2 SetRandomPosition()
    {
        Vector2 _anchorPosition = clothes.GetComponent<RectTransform>().position;
        Vector2 _randomOffest = new Vector2(Random.Range(-randomRangeX, randomRangeX), Random.Range(-randomRangeY, randomRangeY));
        return _anchorPosition + _randomOffest;
    }
}