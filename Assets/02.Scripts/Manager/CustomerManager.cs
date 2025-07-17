using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    private static CustomerManager instance;
    public static CustomerManager Instance {  get { return instance; } }

    public GameObject customerPrefab;
    private Transform door;

    public List<Transform> lines = new List<Transform>();
    private List<CustomerController> customers = new List<CustomerController>();

    private const int customerMaxCount = 5;
    private int customerCount = 0;

    private float currentTime = 0.0f;
    private float appearedTime = 5.0f;

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
        if (customerCount >= 5)
            return;

        customerCount++;
    }

    // ¼Õ´Ô ÅðÀå
    public void DequeueCustomer()
    {

        customerCount--;
    }
}