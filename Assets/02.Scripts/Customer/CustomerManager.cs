using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Utils.EnumTypes;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    private static CustomerManager instance;
    public static CustomerManager Instance {  get { return instance; } }

    public Transform door;
    public GameObject[] customerPrefab;
    public GameObject basketPrefab;
    private List<Transform> basketTranform;

    private List<Transform> counterZone = new List<Transform>();
    public Transform[] completeZone;

    [SerializeField]
    private List<CustomerBehaviour> counterCustomers = new List<CustomerBehaviour>();
    [SerializeField]
    public List<CustomerBehaviour> completeZoneCustomers = new List<CustomerBehaviour>(5);

    private const int customerMaxCount = 5;
    private int counterZoneCustomerCount = 0;
    private int completeZoneCustomerCount = 0;
    private int totalCustomerCount = 0;

    private float currentTime = 0.0f;
    private float appearedTime = 10.0f;

    public bool[] isLaundryFull = new bool[5];

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        Init();
    }

    private void Init()
    {
        door = GameObject.Find("Door").GetComponent<Transform>();
        counterZone = GameObject.Find("CounterLine").GetComponentsInChildren<Transform>().ToList();
        counterZone.Remove(counterZone[0]);
        basketTranform = GameObject.Find("BasketZone").transform.GetChild(0).GetComponentsInChildren<Transform>().ToList();
        basketTranform.RemoveAt(0);
    }

    private void Update()
    {
        if (counterZoneCustomerCount + completeZoneCustomerCount >= customerMaxCount)
            return;

        if(currentTime < appearedTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            int _customerIndex = Random.Range(0, customerPrefab.Length);
            EnqueueCustomer(Instantiate(customerPrefab[_customerIndex], door.position, Quaternion.identity), _customerIndex);
        }
    }

    // 손님 등장 카운터로 이동
    public void EnqueueCustomer(GameObject _customer, int _index)
    {
        if (counterCustomers.Count >= 5)
            return;

        counterZoneCustomerCount++;
        totalCustomerCount++;

        string _frontStr = (_index + 1).ToString();
        string _backStr = totalCustomerCount.ToString("D3");

        counterCustomers.Add(_customer.GetComponent<CustomerBehaviour>());
        counterCustomers[counterZoneCustomerCount - 1].type = (totalCustomerCount % 6 == 0) ? CustomerType.EventCustomer : CustomerType.EventCustomer;
        counterCustomers[counterZoneCustomerCount - 1].customerUID = int.Parse(_frontStr + _backStr);
        counterCustomers[counterZoneCustomerCount - 1].state = CustomerState.CounterZone;
        counterCustomers[counterZoneCustomerCount - 1].lineIndex = counterZoneCustomerCount - 1;
        counterCustomers[counterZoneCustomerCount - 1].SetDestination(counterZone[counterZoneCustomerCount - 1]);
    }

    // 빨래 주문 실행
    public void OnOrder(int _index)
    {
        for(int i = 0; i < isLaundryFull.Length; i++)
        {
            if (!isLaundryFull[i])
            {
                isLaundryFull[i] = true;

                completeZoneCustomers[i] = counterCustomers[_index];
                counterCustomers.RemoveAt(_index);

                GameObject _basket = Instantiate(basketPrefab, basketTranform[i].position, Quaternion.identity);
                BasketController _basketController = _basket.GetComponent<BasketController>();
                _basketController.customerUID = completeZoneCustomers[i].customerUID;
                _basketController.laundryCount = completeZoneCustomers[i].laundryCount;
                _basketController.laundryZoneIndex = i;

                completeZoneCustomers[i].lineIndex = i;
                completeZoneCustomers[i].state = CustomerState.CompleteZone;
                MoveCompleteZone(completeZoneCustomers[i].lineIndex);

                return;
            }
        }
    }

    // 이벤트 주문 실행
    public void OnEventOrder(int _index)
    {
        for (int i = 0; i < isLaundryFull.Length; i++)
        {
            if (!isLaundryFull[i])
            {
                isLaundryFull[i] = true;

                completeZoneCustomers[i] = counterCustomers[_index];
                counterCustomers.RemoveAt(_index);

                completeZoneCustomers[i].lineIndex = i;
                completeZoneCustomers[i].state = CustomerState.CompleteZone;
                MoveCompleteZone(completeZoneCustomers[i].lineIndex);

                return;
            }
        }
    }

    // 손님 대기줄로 이동
    public void MoveCompleteZone(int _index)
    {
        completeZoneCustomerCount++;
        counterZoneCustomerCount--;
        completeZoneCustomers[_index].Init();
        completeZoneCustomers[_index].SetDestination(completeZone[_index]);
        completeZoneCustomers[_index].spriteRenderer.sortingOrder = 1;

        for (int i = 0; i < counterCustomers.Count; i++)
        {
            counterCustomers[i].lineIndex = i;
            counterCustomers[i].SetDestination(counterZone[i]);
        }
    }

    // 손님 가게에서 퇴장
    public void DequeueCustomer(int _index)
    {
        if (counterCustomers.Count <= 0)
            return;

        counterCustomers[_index].Init();
        counterCustomers[_index].SetDestination(door);
        counterCustomers.RemoveAt(_index);
        counterZoneCustomerCount--;

        for (int i = 0; i < counterCustomers.Count; i++)
        {
            counterCustomers[i].lineIndex = i;
            counterCustomers[i].SetDestination(counterZone[i]);
        }
    }

    // 손님 대기줄에서 퇴장
    public IEnumerator OnDelivelySuccess(int _index, GameObject _laundry, GameObject _basket, int _expression)
    {
        Debug.Log(":: 퇴장 시작 ::");
        completeZoneCustomers[_index].speechBubble.SetActive(true);
        completeZoneCustomers[_index].speechBubbles[_expression].SetActive(true);
        completeZoneCustomers[_index].state = (_expression == 1) ? CustomerState.Happy : CustomerState.Angry;
        completeZoneCustomers[_index].animator.SetInteger("Dir", 0);

        yield return new WaitForSeconds(2.5f);

        completeZoneCustomers[_index].speechBubbles[_expression].SetActive(false);
        completeZoneCustomers[_index].state = CustomerState.Leave;
        completeZoneCustomers[_index].Init();
        completeZoneCustomers[_index].spriteRenderer.sortingOrder = 1;
        completeZoneCustomers[_index].SetDestination(door);
        completeZoneCustomers[_index] = null;
        isLaundryFull[_index] = false;
        completeZoneCustomerCount--;
        Destroy(_laundry);
        Destroy(_basket);
    }

    public void CoroutineHandler(int _index, GameObject _laundry, GameObject _basket, int _expression)
    {
        StartCoroutine(OnDelivelySuccess(_index, _laundry, _basket, _expression));
    }
}