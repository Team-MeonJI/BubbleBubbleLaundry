using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Utils.EnumTypes;

public class CustomerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public CustomerState state = CustomerState.Idle;
    private GameObject speechBubble;

    private Vector2 dir;
    private float moveSpeed = 1.0f;
    public int lineIndex = 0;

    public int customerUID = 0;
    private int laundryCount = 0;
    private const int minLaundryCount = 2;
    private const int maxLaundryCount = 4;

    private float currentTime = 0.0f;
    private float waitingTime = 30.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speechBubble = transform.GetChild(0).GetChild(0).gameObject;

        Init();
    }

    public void Init()
    {
        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        speechBubble.SetActive(false);
    }

    private void Update()
    {
        OnDirection();
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
            if (lineIndex == 0)
                speechBubble.SetActive(true);
            else
                speechBubble.SetActive(false);

            spriteRenderer.sortingOrder = lineIndex + 2;
            animator.SetInteger("Dir", 1);
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

    // 방향 확인
    public void OnDirection()
    {
        //if (!agent.hasPath)
        //    return;

        dir = new Vector2(agent.velocity.x, agent.velocity.y).normalized;

        if (dir.sqrMagnitude > 0.01f)
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    // 오른쪽 이동
                    Debug.Log("Right");
                    transform.localScale = new Vector3(1, 1, 1);
                    animator.SetInteger("Dir", 4);
                }
                else
                {
                    // 왼쪽 이동
                    Debug.Log("Left");
                    transform.localScale = new Vector3(-1, 1, 1);
                    animator.SetInteger("Dir", 4);
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    // 위로 이동
                    Debug.Log("Back");
                    animator.SetInteger("Dir", 3);
                }
                else
                {
                    // 아래로 이동
                    Debug.Log("Front");
                    animator.SetInteger("Dir", 2);
                }
            }
        }
        else
        {
            Debug.Log("Front Idle");
            animator.SetInteger("Dir", 0);
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.transform.CompareTag("Door") && state == CustomerState.Leave)
        {
            Destroy(gameObject, 0.25f);
        }
        else if (coll.transform.CompareTag("Line") && state == CustomerState.Move)
        {
            state = CustomerState.Wait;
            laundryCount = Random.Range(minLaundryCount, maxLaundryCount);
        }
    }
}