using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using Utils.EnumTypes;

public class MachineManager : MonoBehaviour
{
    private static MachineManager instance;
    public static MachineManager Instance {  get { return instance; } }

    private RaycastHit2D hit;
    public BasketController basket;
    public MachineController machine;

    public int clickCount = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(_pos, Vector2.zero, 0.0f);

            if (hit.collider != null)
            {
                clickCount++;
                
                if (hit.collider.CompareTag("Machine"))
                    OnMachineSelect();
                else if (hit.collider.CompareTag("Basket"))
                    OnBasketSelect();
                else
                {
                    Debug.Log("::: 선택 실수 :::");
                    if (machine != null)
                    {
                        machine.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                        machine = null;
                        basket = null;
                    }
                    else if (basket != null)
                    {
                        basket.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                        basket = null;
                    }

                    clickCount = 0;
                }
            }
        }
    }

    // 기계 선택
    public void OnMachineSelect()
    {
        MachineController _machine = hit.collider?.GetComponent<MachineController>();

        if (clickCount == 1)
        {
            basket = _machine.currentBasket;
            machine = _machine;
            machine.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (clickCount >= 2 && basket != null && _machine != null)
        {
            if (basket.laundryState == _machine.laundryState && _machine.machineState == MachineState.Idle)
            {
                if (machine != null)
                {
                    machine.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    machine.Init();
                    machine = null;
                }

                _machine.currentBasket = basket;
                _machine.SetTime(basket.laundryCount);
                _machine.machineState = MachineState.Working;

                basket.OnNextStep();
                basket.gameObject.SetActive(false);
                clickCount = 0;
            }
            else
            {
                if (machine != null)
                {
                    machine.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    machine = null;
                    basket = null;
                }
                clickCount = 0;
            }
        }
    }

    // 빨랫 바구니 선택
    public void OnBasketSelect()
    {
        if (basket != null)
        {
            basket.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            basket = hit.collider?.GetComponent<BasketController>();
            basket.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            basket = hit.collider?.GetComponent<BasketController>();
            basket.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}