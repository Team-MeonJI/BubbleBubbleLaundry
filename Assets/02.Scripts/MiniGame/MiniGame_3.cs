using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils.EnumTypes;

public class MiniGame_3 : MiniGameController
{
    public GameObject arrowPrefab;
    public List<GameObject> arrows = new List<GameObject>();

    private GameObject command;
    private GameObject arrowGrid;
    private ArrowController commandArrow;
    private TextMeshProUGUI remainArrowCount;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (currentTime > 0)
        {
            OnClickArrow();
            currentTime -= Time.deltaTime;
            timerText.text = ((int)currentTime).ToString();
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
        command = transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        arrowGrid = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).gameObject;
        remainArrowCount = transform.GetChild(0).GetChild(1).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

        for (int i = 0; i < 20; i++)
        {
            arrows.Add(Instantiate(arrowPrefab, arrowGrid.transform));
        }

        commandArrow = arrows[0].GetComponent<ArrowController>();
        commandArrow.transform.parent = command.transform;
        commandArrow.GetComponent<RectTransform>().localPosition = Vector3.zero;
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

    // 방향키 입력 확인
    public void OnClickArrow()
    {
        if (arrows.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && commandArrow.arrowType == ArrowType.Up ||
                Input.GetKeyDown(KeyCode.DownArrow) && commandArrow.arrowType == ArrowType.Down ||
                Input.GetKeyDown(KeyCode.RightArrow) && commandArrow.arrowType == ArrowType.Right ||
                Input.GetKeyDown(KeyCode.LeftArrow) && commandArrow.arrowType == ArrowType.Left)
            {
                Destroy(commandArrow.gameObject);
                arrows.RemoveAt(0);
                remainArrowCount.text = (arrows.Count).ToString();

                if (arrows.Count != 0)
                {
                    commandArrow = arrows[0].GetComponent<ArrowController>();
                    commandArrow.transform.parent = command.transform;
                    commandArrow.GetComponent<RectTransform>().localPosition = Vector3.zero;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && commandArrow.arrowType != ArrowType.Up ||
                Input.GetKeyDown(KeyCode.DownArrow) && commandArrow.arrowType != ArrowType.Down ||
                Input.GetKeyDown(KeyCode.RightArrow) && commandArrow.arrowType != ArrowType.Right ||
                Input.GetKeyDown(KeyCode.LeftArrow) && commandArrow.arrowType != ArrowType.Left)
            {

            }
        }
        else
        {
            Debug.Log("::: Game Over :::");
            isGameSuccess = true;
            MiniGameEnd();
        }
    }
}