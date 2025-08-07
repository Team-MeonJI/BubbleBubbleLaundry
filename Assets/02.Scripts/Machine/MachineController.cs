using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.EnumTypes;

public class MachineController : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    private GameObject timer;
    private Slider timerSlider;

    private SpriteRenderer laundryObject;
    public Sprite[] laundrySprites;

    public MachineType machineType;
    public LaundryState laundryState;
    public MachineState machineState = MachineState.Idle;
    public BasketController currentBasket;

    private float currentTime = 0;
    public float operationTime = 5;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        timer = transform.GetChild(0).GetChild(0).gameObject;
        timerSlider = timer.GetComponent<Slider>();
        laundryObject = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (machineState)
        {
            case MachineState.Idle:
                Init();
                break;
            case MachineState.Working:
                StartCoroutine(OnOperation());
                break;
            case MachineState.Complete:
                break;
        }
    }

    public void Init()
    {
        StopAllCoroutines();
        machineState = MachineState.Idle;

        OnLaundryHandler(false);
        timer.SetActive(false);

        currentBasket = null;
        currentTime = 0.0f;

        animator.SetBool("Select", false);
        animator.SetBool("Action", false);
    }

    // 작동 시간 설정
    public void SetTime(int _laundryCount)
    {
        operationTime += (_laundryCount - 1) * 1.5f;
    }

    // 기계 작동 시작
    private IEnumerator OnOperation()
    {
        if (machineType == MachineType.IroningBoard)
            OnLaundryHandler(true);

        yield return new WaitForSeconds(0.433f);

        timer.SetActive(true);

        if(currentTime < operationTime && machineState == MachineState.Working)
        {
            currentTime += Time.deltaTime;
            timerSlider.value = (float)(currentTime / operationTime);
        }
        else
        {
            currentTime = 0;
            timer.SetActive(false);
            animator.SetBool("Action", false);
            machineState = MachineState.Complete;

            yield return new WaitForSeconds(0.3f);

            OnLaundryHandler(true);
        }
    }

    // 기계 선택
    public void OnSelect(bool isActive)
    {
        if (!isActive)
            animator.SetBool("Select", false);
        else
            animator.SetBool("Select", true);
    }

    // 완료된 빨랫감 관리
    public void OnLaundryHandler(bool isActive)
    {
        if (isActive && currentBasket != null)
            laundryObject.sprite = laundrySprites[currentBasket.spriteIndex];
        laundryObject.gameObject.SetActive(isActive);
    }
}