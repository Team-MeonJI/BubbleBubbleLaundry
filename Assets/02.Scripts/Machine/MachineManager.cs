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

    private int clickCount = 0;

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
                {
                    OnMachineSelect();
                }
                else if (hit.collider.CompareTag("Basket"))
                {
                    OnBasketSelect();
                }
            }
        }
    }

    // ±‚∞Ë º±≈√
    public void OnMachineSelect()
    {
        MachineController _machine = hit.collider?.GetComponent<MachineController>();

        if (clickCount == 1)
        {
            basket = _machine.currentBasket;
            machine = _machine;
            machine.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (basket != null && _machine != null)
        {
            if ((basket.laundryState == _machine.laundryState) && _machine.machineState == MachineState.Idle)
            {
                Debug.Log(_machine.machineType.ToString() + "ºº≈π Ω√¿€");
                if (machine != null)
                    machine.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

                _machine.currentBasket = basket;
                _machine.SetTime(basket.laundryCount);
                _machine.machineState = MachineState.Working;
                basket.OnNextStep();
                basket.gameObject.SetActive(false);
                clickCount = 0;
            }
        }
        else
        {
            if (basket != null)
            {
                basket.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                basket = null;
            }
        }
    }

    // ª°∑ß πŸ±∏¥œ º±≈√
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