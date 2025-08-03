using System.Collections.Generic;
using UnityEngine;

public class MiniGame_3 : MiniGameController
{
    public GameObject arrowPrefab;
    public Queue<GameObject> arrows = new Queue<GameObject>();

    private GameObject command;
    private GameObject arrowGrid;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        command = transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        arrowGrid = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).gameObject;

        for (int i = 0; i < 20; i++)
        {
            arrows.Enqueue(Instantiate(arrowPrefab, arrowGrid.transform));
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
    }
}