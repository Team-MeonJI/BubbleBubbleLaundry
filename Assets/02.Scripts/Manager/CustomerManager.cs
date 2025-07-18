using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Utils.EnumTypes;

public class CustomerManager : MonoBehaviour
{
    private static CustomerManager instance;
    public static CustomerManager Instance {  get { return instance; } }

    public GameObject customerPrefab;
    private Transform door;

    private List<Transform> lines = new List<Transform>();
    private List<CustomerController> customers = new List<CustomerController>();

    private const int customerMaxCount = 5;
    private int customerCount = 0;

    private float currentTime = 0.0f;
    private float appearedTime = 10.0f;

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
        lines = GameObject.Find("Line").GetComponentsInChildren<Transform>().ToList();
        lines.Remove(lines[0]);
    }

    private void Update()
    {
        if (customerCount >= customerMaxCount)
            return;

        if(currentTime < appearedTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            EnqueueCustomer(Instantiate(customerPrefab, door.position, Quaternion.identity).GetComponent<CustomerController>());
        }
    }

    // ¼Õ´Ô µîÀå
    public void EnqueueCustomer(CustomerController _customer)
    {
        if (customers.Count >= 5)
            return;

        customerCount++;
        customers.Add(_customer);
        customers[customerCount - 1].state = CustomerState.Move;
        customers[customerCount - 1].lineIndex = customerCount - 1;
        customers[customerCount - 1].SetDestination(lines[customerCount - 1]);
    }

    // ¼Õ´Ô ÅðÀå
    public void DequeueCustomer(int _index)
    {
        if (customers.Count <= 0)
            return;

        customers[_index].SetDestination(door);
        customers.RemoveAt(_index);
        customerCount--;

        for (int i = 0; i < customers.Count; i++)
        {
            customers[i].lineIndex = i;
            customers[i].SetDestination(lines[i]);
        }
    }
}