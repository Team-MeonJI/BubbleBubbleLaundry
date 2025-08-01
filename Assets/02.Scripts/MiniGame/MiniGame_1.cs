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

    private void Awake()
    {
        clothes = transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        spotCount = Random.Range(minSpotCount, maxSpotCount);
        Init();

        for (int i = 0; i < spotCount; i++)
        {
            spots.Add(Instantiate(spotPrefab, clothes.transform));
            spots[i].transform.position = SetRandomPosition();
        }
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if(spots.Count <= 0)
        {
            isGameSuccess = true;
            MiniGameEnd();
        }
    }

    public override void Init()
    {
        base.Init();
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
    }

    // ��� ���� ��ġ ����
    public Vector2 SetRandomPosition()
    {
        Vector2 _anchorPosition = clothes.GetComponent<RectTransform>().position;
        Vector2 _randomOffest = new Vector2(Random.Range(-randomRangeX, randomRangeX), Random.Range(-randomRangeY, randomRangeY));
        return _anchorPosition + _randomOffest;
    }
}