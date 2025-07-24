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
    private List<CustomerBehaviour> completeZoneCustomers = new List<CustomerBehaviour>();

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

    // º’¥‘ µÓ¿Â ƒ´øÓ≈Õ∑Œ ¿Ãµø
    public void EnqueueCustomer(GameObject _customer, int _index)
    {
        if (counterCustomers.Count >= 5)
            return;

        counterZoneCustomerCount++;
        totalCustomerCount++;

        string _frontStr = (_index + 1).ToString();
        string _backStr = totalCustomerCount.ToString("D3");

        counterCustomers.Add(_customer.GetComponent<CustomerBehaviour>());
        counterCustomers[counterZoneCustomerCount - 1].customerUID = int.Parse(_frontStr + _backStr);
        counterCustomers[counterZoneCustomerCount - 1].state = CustomerState.CounterZone;
        counterCustomers[counterZoneCustomerCount - 1].lineIndex = counterZoneCustomerCount - 1;
        counterCustomers[counterZoneCustomerCount - 1].SetDestination(counterZone[counterZoneCustomerCount - 1]);
    }

    // ¡÷πÆ Ω««‡
    public void OnOrder(int _index)
    {
        for(int i = 0; i < isLaundryFull.Length; i++)
        {
            if (!isLaundryFull[i])
            {
                isLaundryFull[i] = true;

                completeZoneCustomers.Add(counterCustomers[_index]);
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

    // º’¥‘ ¥Î±‚¡Ÿ∑Œ ¿Ãµø
    public void MoveCompleteZone(int _index)
    {
        completeZoneCustomerCount++;
        counterZoneCustomerCount--;
        completeZoneCustomers[_index].Init();
        completeZoneCustomers[_index].SetDestination(completeZone[_index]);

        for (int i = 0; i < counterCustomers.Count; i++)
        {
            counterCustomers[i].lineIndex = i;
            counterCustomers[i].SetDestination(counterZone[i]);
        }
    }

    // º’¥‘ ¥Î±‚¡Ÿø°º≠ ≈¿Â
    public void DequeueCompleteZone(int _index)
    {
        Debug.Log("≈¿Â");
        completeZoneCustomers[_index].state = CustomerState.Leave;
        completeZoneCustomers[_index].Init();
        completeZoneCustomers[_index].SetDestination(door);
        completeZoneCustomers.RemoveAt(_index);
        isLaundryFull[_index] = false;
        completeZoneCustomerCount--;
    }

    // º’¥‘ ∞°∞‘ø°º≠ ≈¿Â
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

    // º’¥‘ ª°∑° πﬁ∞Ì ≈¿Â
    public IEnumerator OnDelivelySuccess(int _index, GameObject _laundry)
    {
        Debug.Log(":: ≈¿Â Ω√¿€ ::");
        completeZoneCustomers[_index].speechBubble[1].SetActive(true);
        completeZoneCustomers[_index].state = CustomerState.Happy;
        completeZoneCustomers[_index].animator.SetInteger("Dir", 0);

        yield return new WaitForSeconds(2.5f);

        completeZoneCustomers[_index].speechBubble[1].SetActive(false);
        DequeueCompleteZone(_index);
        Destroy(_laundry);
    }

    public void CoroutineHandler(int _index, GameObject _laundry)
    {
        StartCoroutine(OnDelivelySuccess(_index, _laundry));
    }
}