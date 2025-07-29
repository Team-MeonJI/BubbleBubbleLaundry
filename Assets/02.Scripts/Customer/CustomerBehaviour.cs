using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Utils.EnumTypes;
using UnityEngine.EventSystems;
using System.Collections;

public class CustomerBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Camera uiCam;
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    public GameObject speechBubble;
    public GameObject[] speechBubbles;

    public CustomerState state = CustomerState.Idle;
    private Vector2 dir;
    private float moveSpeed = 1.0f;
    public int lineIndex = 0;

    public int customerUID = 0;
    public int laundryCount = 0;
    private const int minLaundryCount = 2;
    private const int maxLaundryCount = 5;

    private float currentTime = 0.0f;
    private float waitingTime = 30.0f;
    private float laundryWaitingTime = 50.0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        canvas = transform.GetChild(0).GetComponent<Canvas>();
        uiCam = Camera.main;
        canvas.worldCamera = uiCam;
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        speechBubble = canvas.transform.GetChild(0).gameObject;

        Init();
    }

    public void Init()
    {
        currentTime = 0.0f;
        spriteRenderer.sortingOrder = 10;

        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        speechBubble.SetActive(false);
        for (int i = 0; i < speechBubbles.Length; i++)
            speechBubbles[i].SetActive(false);
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
            case CustomerState.CounterZone:
                break;
            case CustomerState.CompleteZone:
                break;
            case CustomerState.Wait:
                spriteRenderer.sortingOrder = lineIndex + 10;
                OnWaiting();
                break;
            case CustomerState.LaundryWait:
                OnLaundryWaiting();
                break;
            case CustomerState.Happy:
                break;
            case CustomerState.Angry:
                break;
            case CustomerState.Leave:
                break;
        }
    }

    // 목적지 설정
    public void SetDestination(Transform _target)
    {
        agent.SetDestination(_target.position);
    }

    // 주문 기다리는 중
    public void OnWaiting()
    {
        if (lineIndex != 0)
            return;

        if (currentTime < waitingTime)
        {
            speechBubble.SetActive(true);
            speechBubbles[0].SetActive(true);
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0.0f;
            state = CustomerState.Leave;
            CustomerManager.Instance.DequeueCustomer(lineIndex);
        }

        if (OnRayCheck()?.name == "Bubble")
            CustomerManager.Instance.OnOrder(lineIndex);
    }

    // 빨래 기다리는 중
    public void OnLaundryWaiting()
    {
        if (currentTime < laundryWaitingTime)
        {
            animator.SetInteger("Dir", 1);
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0.0f;
            state = CustomerState.Angry;
            CustomerManager.Instance.CoroutineHandler(lineIndex, null, 2);
        }
    }

    // 방향 확인
    public void OnDirection()
    {
        if (state == CustomerState.Happy || state == CustomerState.Angry)
            return;

        dir = new Vector2(agent.velocity.x, agent.velocity.y).normalized;
        Debug.DrawRay(transform.position, dir * 1.5f, Color.red);

        if (dir.sqrMagnitude > 0.01f)
        {
            if (dir.y > 0.33f)
                animator.SetInteger("Dir", 3);
            else if (dir.y < -0.33f)
                animator.SetInteger("Dir", 2);
            else if (dir.x > 0.0f || dir.x < 0.0f)
            {
                transform.localScale = new Vector3(((dir.x > 0.0) ? -1 : 1), 1, 1);
                animator.SetInteger("Dir", 4);
            }
        }
    }

    // Ray 충돌 확인
    public GameObject OnRayCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count > 0)
                return results[0].gameObject;
            else
                return null;
        }
        else
            return null;
    }

    // AI가 목적지에 도착했는지 확인용
    private bool HasReacheDestination()
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
        if (coll.transform.CompareTag("Line") && state == CustomerState.CounterZone)
        {
            spriteRenderer.sortingOrder = lineIndex + 10;

            if (HasReacheDestination())
            {
                state = CustomerState.Wait;
                animator.SetInteger("Dir", 1);
                laundryCount = Random.Range(minLaundryCount, maxLaundryCount);
            }
        }
        if (coll.transform.CompareTag("CompleteZone") && HasReacheDestination())
        {
            if (state == CustomerState.CompleteZone)
            {
                spriteRenderer.sortingOrder = 10;
                state = CustomerState.LaundryWait;
            }
        }
    }
}