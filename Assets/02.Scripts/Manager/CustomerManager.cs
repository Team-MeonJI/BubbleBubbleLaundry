using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Utils.EnumTypes;

public class CustomerManager : MonoBehaviour
{
    private static CustomerManager instance;
    public static CustomerManager Instance {  get { return instance; } }

    private Transform door;
    public GameObject[] customerPrefab;
    public GameObject basketPrefab;
    private List<Transform> basketTranform;

    private List<Transform> counterLines = new List<Transform>();
    private List<Transform> completeZones;
    private List<CustomerBehaviour> counterCustomers = new List<CustomerBehaviour>();
    private List<CustomerBehaviour> completeZoneCustomers = new List<CustomerBehaviour>();

    private const int customerMaxCount = 5;
    public int lineCustomerCount = 0;
    public int completeLineCount = 0;
    private int totalCustomerCount = 0;

    private float currentTime = 0.0f;
    private float appearedTime = 10.0f;

    private bool[] laundryFull = new bool[5];

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
        counterLines = GameObject.Find("CounterLine").GetComponentsInChildren<Transform>().ToList();

        basketTranform = GameObject.Find("BasketZone").transform.GetChild(0).GetComponentsInChildren<Transform>().ToList();
        basketTranform.RemoveAt(0);
        completeZones = GameObject.Find("CompleteZone").transform.GetChild(2).GetComponentsInChildren<Transform>().ToList();
        completeZones.RemoveAt(0);
        counterLines.Remove(counterLines[0]);
    }

    private void Update()
    {
        if (lineCustomerCount + completeLineCount >= customerMaxCount)
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

    // º’¥‘ µÓ¿Â
    public void EnqueueCustomer(GameObject _customer, int _index)
    {
        if (counterCustomers.Count >= 5)
            return;

        lineCustomerCount++;
        totalCustomerCount++;

        string _frontStr = (_index + 1).ToString();
        string _backStr = totalCustomerCount.ToString("D3");

        counterCustomers.Add(_customer.GetComponent<CustomerBehaviour>());
        counterCustomers[lineCustomerCount - 1].customerUID = int.Parse(_frontStr + _backStr);
        counterCustomers[lineCustomerCount - 1].state = CustomerState.Move;
        counterCustomers[lineCustomerCount - 1].lineIndex = lineCustomerCount - 1;
        counterCustomers[lineCustomerCount - 1].SetDestination(counterLines[lineCustomerCount - 1]);
    }

    // º’¥‘ ¿Ãµø
    public void MoveCustomer(int _index)
    {
        completeZoneCustomers[_index].Init();
        completeZoneCustomers[_index].SetDestination(completeZones[completeZoneCustomers[_index].lineIndex]);
        completeLineCount++;
        lineCustomerCount--;

        for (int i = 0; i < counterCustomers.Count; i++)
        {
            counterCustomers[i].lineIndex = i;
            counterCustomers[i].SetDestination(counterLines[i]);
        }
    }

    // ª°∑ß∞® ¡÷πÆ
    public void OnOrder(int _index)
    {
        for(int i = 0; i < laundryFull.Length; i++)
        {
            if (!laundryFull[i])
            {
                laundryFull[i] = true;
                completeZoneCustomers.Add(counterCustomers[_index]);
                counterCustomers.RemoveAt(_index);

                GameObject _basket = Instantiate(basketPrefab, basketTranform[i].position, Quaternion.identity);
                _basket.GetComponent<BasketController>().customerUID = completeZoneCustomers[_index].customerUID;

                completeZoneCustomers[_index].lineIndex = i;
                completeZoneCustomers[_index].state = CustomerState.WaitLine;
                MoveCustomer(completeZoneCustomers[_index].lineIndex);

                return;
            }
        }
    }

    // º’¥‘ ≈¿Â
    public void DequeueCustomer(int _index)
    {
        if (counterCustomers.Count <= 0)
            return;

        counterCustomers[_index].Init();
        counterCustomers[_index].SetDestination(door);
        counterCustomers.RemoveAt(_index);
        lineCustomerCount--;

        for (int i = 0; i < counterCustomers.Count; i++)
        {
            counterCustomers[i].lineIndex = i;
            counterCustomers[i].SetDestination(counterLines[i]);
        }
    }
}