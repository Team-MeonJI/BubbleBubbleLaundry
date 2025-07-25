using UnityEngine;
using UnityEngine.UI;
using Utils.EnumTypes;

public class MachineController : MonoBehaviour
{
    private GameObject timer;
    private Slider timerSlider;

    public MachineType machineType;
    public LaundryState laundryState;
    public MachineState machineState = MachineState.Idle;
    public BasketController currentBasket;

    private float currentTime = 0;
    public float operationTime = 5;

    private void Start()
    {
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
}