using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private float moveSpeed = 2.5f;
    private int laundryCount = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = moveSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {

    }

    public void SetDestination(Transform _target)
    {
        agent.SetDestination(_target.position);
    }
}