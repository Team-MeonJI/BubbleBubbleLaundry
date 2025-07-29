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
                if (hit.collider.CompareTag("Basket"))
                    OnBasketSelect();
                else if (hit.collider.CompareTag("Machine"))
                    OnMachineSelect();
            }
        }
    }

    // ±â°è ¼±ÅÃ
    public void OnMachineSelect()
    {
        MachineController _machine = hit.collider?.GetComponent<MachineController>();

        if(basket == null)
        {
            if (_machine.machineState == MachineState.Complete)
            {
                // »¡·¡ ¿Ï·á µÈ ±â°è¿¡¼­ »¡·§°¨ ²¨³»±â
                machine = _machine;
                machine.OnSelect(true);
                basket = machine.currentBasket;

                if(_machine.machineType == MachineType.IroningBoard)
                {
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
            if(_machine.machineState == MachineState.Idle && basket.laundryState == _machine.laundryState)
            {
                // ºñ¾î ÀÖ´Â ±â°è¿¡ »¡·¡ ³Ö±â
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
                if (machine != null)
                {
                    machine.OnSelect(false);
                    machine = null;
                }
                else
                {
                    basket.OnSelect(false);
                    basket = null;
                }
            }
        }
    }

    // »¡·§ ¹Ù±¸´Ï ¼±ÅÃ
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
}