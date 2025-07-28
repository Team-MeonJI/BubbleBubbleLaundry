using UnityEngine;
using UnityEngine.UI;
using Utils.EnumTypes;

public class MachineController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject timer;
    private Slider timerSlider;

    public Sprite[] machineSprites;
    public Sprite[] selectSprites;

    public MachineType machineType;
    public LaundryState laundryState;
    public MachineState machineState = MachineState.Idle;
    public BasketController currentBasket;

    private float currentTime = 0;
    public float operationTime = 5;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = transform.GetChild(0).GetChild(0).gameObject;
        timerSlider = timer.GetComponent<Slider>();
    }

    private void Update()
    {
        switch (machineState)
        {
            case MachineState.Idle:
                break;
            case MachineState.Working:
                OnOperation();
                break;
            case MachineState.Complete:
                break;
        }
    }

    public void Init()
    {
        timer.SetActive(false);
        machineState = MachineState.Idle;
    }

    // 작동 시간 설정
    public void SetTime(int _laundryCount)
    {
        operationTime += (_laundryCount - 1) * 1.5f;
    }

    // 기계 작동 시작
    public void OnOperation()
    {
        timer.SetActive(true);

        if(currentTime < operationTime)
        {
            currentTime += Time.deltaTime;
            timerSlider.value = (float)(currentTime / operationTime);
        }
        else
        {
            currentTime = 0;
            machineState = MachineState.Complete;
        }
    }

    // 기계 선택
    public void OnSelect(bool isActive, int _index)
    {
        Debug.Log(_index);

        if (!isActive)
            spriteRenderer.sprite = machineSprites[_index];
        else
            spriteRenderer.sprite = selectSprites[_index];
    }
}