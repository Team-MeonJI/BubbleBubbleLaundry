using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils.EnumTypes;

public class MiniGame_3 : MiniGameController
{
    public GameObject arrowPrefab;
    public List<GameObject> arrows = new List<GameObject>();

    private Animator animator;
    private GameObject command;
    private GameObject arrowGrid;
    private ArrowController commandArrow;
    private TextMeshProUGUI remainArrowCount;

    private const int reward = 150;

    private void Awake()
    {
        animator = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Animator>();
        command = transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        arrowGrid = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).gameObject;
        remainArrowCount = transform.GetChild(0).GetChild(1).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
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

        isGameOver = false;
        isGameSuccess = false;
        currentTime = miniGameTime;
        resultPhanel.SetActive(false);

        for (int i = 0; i < 20; i++)
        {
            arrows.Add(Instantiate(arrowPrefab, arrowGrid.transform));
        }

        commandArrow = arrows[0].GetComponent<ArrowController>();
        commandArrow.transform.parent = command.transform;
        remainArrowCount.text = (arrows.Count).ToString();
        SetArrow();
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
                animator.SetTrigger("Sewing");
                Destroy(commandArrow.gameObject);
                arrows.RemoveAt(0);
                remainArrowCount.text = (arrows.Count).ToString();

                if (arrows.Count != 0)
                {
                    commandArrow = arrows[0].GetComponent<ArrowController>();
                    commandArrow.transform.parent = command.transform;
                    SetArrow();
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

    // 화살표 위치, 크기 변경
    public void SetArrow()
    {
        RectTransform _rect = commandArrow.GetComponent<RectTransform>();
        _rect.localPosition = new Vector3(0.0f, -13.0f, 0.0f);

        Vector2 _size = _rect.sizeDelta;
        _size.x = 85.0f;
        _size.y = 85.0f;
        _rect.sizeDelta = _size;
    }
}