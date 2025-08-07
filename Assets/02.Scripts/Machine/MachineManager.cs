using UnityEngine;
using UnityEngine.EventSystems;
using Utils.EnumTypes;

public class MachineManager : MonoBehaviour
{
    private static MachineManager instance;
    public static MachineManager Instance {  get { return instance; } }

    private RaycastHit2D hit;
    public BasketController basket;
    public MachineController machine;
    public GameObject[] machines;

    public int clickCount = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        machines = GameObject.FindGameObjectsWithTag("Machine");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(_pos, Vector2.zero, 0.0f);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Basket"))
                    OnBasketSelect();
                else if (hit.collider.CompareTag("Machine"))
                    OnMachineSelect();
            }
        }
    }

    // 기계 선택
    public void OnMachineSelect()
    {
        MachineController _machine = hit.collider?.GetComponent<MachineController>();

        if(basket == null)
        {
            if (_machine.machineState == MachineState.Complete)
            {
                // 빨래를 완료한 기계 선택
                machine = _machine;
                machine.OnSelect(true);
                basket = machine.currentBasket;

                if(_machine.machineType == MachineType.IroningBoard)
                {
                    // 작동을 끝낸 다리미 선택
                    basket.OnComplete();
                    _machine.Init();
                    machine.OnSelect(false);
                    machine.Init();
                    machine = null;
                    basket = null;
                }
            }
        }
        else if(basket != null)
        {
            if(_machine.machineState == MachineState.Idle)
            {
                if(basket.laundryState == _machine.laundryState)
                {
                    // 이미 이전 단계의 기계를 선택했으며 비어 있는 기계를 선택
                    if (machine != null)
                    {
                        _machine.currentBasket = basket;
                        _machine.SetTime(basket.laundryCount);
                        _machine.animator.SetBool("Action", true);
                        _machine.machineState = MachineState.Working;

                        machine.OnSelect(false);
                        machine.Init();
                        machine = null;

                        basket.OnNextStep();
                        basket.gameObject.SetActive(false);
                        basket = null;
                    }
                    else
                    {
                        _machine.currentBasket = basket;
                        _machine.SetTime(basket.laundryCount);
                        _machine.animator.SetBool("Action", true);
                        _machine.machineState = MachineState.Working;

                        _machine.OnSelect(false);
                        machine = null;

                        basket.OnNextStep();
                        basket.gameObject.SetActive(false);
                        basket = null;
                    }
                }
                else
                {
                    StartCoroutine(UIManager.Instance.OnException(1));

                    if (machine != null)
                    {
                        machine.OnSelect(false);
                        machine = null;
                        basket = null;
                    }
                    else
                    {
                        basket.OnSelect(false);
                        basket = null;
                    }
                }
            }
            else
            {
                StartCoroutine(UIManager.Instance.OnException(0));

                if (machine != null)
                {
                    machine.OnSelect(false);
                    machine = null;
                    basket = null;
                }
                else
                {
                    basket.OnSelect(false);
                    basket = null;
                }
            }
        }
    }

    // 빨랫 바구니 선택
    public void OnBasketSelect()
    {
        if (machine != null)
        {
            machine.GetComponent<BasketController>().OnSelect(false);
            machine = null;
        }

        if (basket == null)
        {
            basket = hit.collider?.GetComponent<BasketController>();
            basket.OnSelect(true);
        }
        else
        {
            basket.OnSelect(false);
            basket = hit.collider?.GetComponent<BasketController>();
            basket.OnSelect(true);
        }
    }

    // 기계 확인
    public void OnMachineCheck(int _UID)
    {
        for(int i = 0; i < 10; i++)
        {
            MachineController _machine = machines[i].GetComponent<MachineController>();

            if(_machine.currentBasket !=  null)
            {
                if (_machine.currentBasket.customerUID == _UID)
                {
                    GameObject _basket = _machine.currentBasket.gameObject;
                    _machine.Init();
                    Destroy(_basket);
                    return;
                }
            }
        }
    }
}