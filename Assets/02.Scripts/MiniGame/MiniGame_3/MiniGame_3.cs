using TMPro;
using UnityEngine;
using Utils.EnumTypes;
using System.Collections;
using System.Collections.Generic;

public class MiniGame_3 : MiniGameController
{
    private AudioSource audioSource;
    private AudioSource sewingAudioSource;

    public GameObject arrowPrefab;
    public List<GameObject> arrows = new List<GameObject>();

    private Animator animator;
    private GameObject command;
    private GameObject arrowGrid;
    private ArrowController commandArrow;
    private TextMeshProUGUI remainArrowCount;
    public GameObject error;

    private const int reward = 1500;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sewingAudioSource = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<AudioSource>();
        animator = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Animator>();
        command = transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        arrowGrid = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).gameObject;
        remainArrowCount = transform.GetChild(0).GetChild(1).GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        error = transform.GetChild(0).GetChild(3).gameObject;
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            isGameOver = true;
            isGameSuccess = false;
            transform.gameObject.SetActive(false);
            GameManager.Instance.ReputationHandler(-GameManager.Instance.reputeDecr);
            return;
        }

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
            StartCoroutine(GameEnd());
        }
    }

    public override void Init()
    {
        base.Init();

        isGameOver = false;
        isGameSuccess = false;
        currentTime = miniGameTime;
        resultPhanel.SetActive(false);

        for (int i = 0; i < 21; i++)
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
            UIManager.Instance.ChangeSewingMachineText();
            GameManager.Instance.ReputationHandler(GameManager.Instance.reputeAdd);
            GameManager.Instance.MoneyHandler(reward * (int)currentTime);
        }
        else
        {
            GameManager.Instance.ReputationHandler(-GameManager.Instance.reputeDecr);
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
                audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.MiniGame2_Input];
                audioSource.Play();

                animator.SetTrigger("Sewing");
                sewingAudioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.MiniGame2_Sewing];
                sewingAudioSource.Play();

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
                StartCoroutine(OnError());
                sewingAudioSource.clip = null;
                audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.MiniGame2_Error];
                audioSource.Play();
            }
        }
        else
        {
            Debug.Log("::: Game Over :::");
            sewingAudioSource.clip = null;
            isGameSuccess = true;
            StartCoroutine(GameEnd());
        }
    }

    // 잘못 입력했을 경우
    public IEnumerator OnError()
    {
        error.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        error.SetActive(false);
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