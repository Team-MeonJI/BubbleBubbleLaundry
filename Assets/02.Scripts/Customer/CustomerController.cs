using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Utils.EnumTypes;

public class CustomerController : MonoBehaviour
{
    private NavMeshAgent agent;

    public CustomerState state = CustomerState.Idle;
    private GameObject basket;
    private TextMeshProUGUI laundryCountText;

    private float moveSpeed = 2.5f;
    public int lineIndex = 0;
    private int laundryCount = 0;
    private const int minLaundryCount = 1;
    private const int maxLaundryCount = 5;

    private float currentTime = 0.0f;
    private float waitingTime = 30.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        basket = transform.GetChild(0).GetChild(0).gameObject;
        laundryCountText = basket.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>(true);

        Init();
    }

    private void Init()
    {
        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        StateHandler();
    }

    private void StateHandler()
    {
        switch (state)
        {
            case CustomerState.Idle:
                break;
            case CustomerState.Move:
                break;
            case CustomerState.Wait:
                OnWaiting();
                break;
            case CustomerState.Leave:
                break;
        }
    }

    // 기다기는 중
    public void OnWaiting()
    {
        if(currentTime < waitingTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0.0f;
            state = CustomerState.Leave;
            CustomerManager.Instance.DequeueCustomer(lineIndex);
        }
    }

    // 목적지 설정
    public void SetDestination(Transform _target)
    {
        agent.SetDestination(_target.position);
    }

    // 목적지 도착 확인
    public bool HasArriveDestination()
    {
        // 경로 계산이 끝났다면
        if (!agent.pathPending)
        {
            // 남은 거리가 짧다면
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // 움직이지 않는 상태라면
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0.0f)
                    return true;
            }
        }
        return false;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.transform.CompareTag("Door") && state == CustomerState.Leave)
        {
            Destroy(gameObject, 0.25f);
        }
        else if(coll.transform.CompareTag("Line") && state == CustomerState.Move)
        {
            state = CustomerState.Wait;
            laundryCount = Random.Range(minLaundryCount, maxLaundryCount);
            laundryCountText.text = laundryCount.ToString();
            basket.SetActive(true);
        }
    }
}