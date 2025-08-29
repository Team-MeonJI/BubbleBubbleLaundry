using UnityEngine;
using Utils.EnumTypes;
using UnityEngine.EventSystems;

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
        if (GameManager.Instance.isGameOver)
            return;

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

    // ��� ����
    public void OnMachineSelect()
    {
        MachineController _machine = hit.collider?.GetComponent<MachineController>();

        if(basket == null)
        {
            if (_machine.machineState == MachineState.Complete)
            {
                // ������ �Ϸ��� ��� ����
                _machine.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.Click];
                _machine.audioSource.Play();
                machine = _machine;
                machine.OnSelect(true);
                basket = machine.currentBasket;

                //if(_machine.machineType == MachineType.IroningBoard)
                //{
                //    // �۵��� ���� �ٸ��� ����
                //    _machine.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.Click];
                //    _machine.audioSource.Play();
                //    basket.OnComplete();
                //    _machine.Init();
                //    machine.OnSelect(false);
                //    machine.Init();
                //    machine = null;
                //    basket = null;
                //}
            }
        }
        else if(basket != null)
        {
            if(_machine.machineState == MachineState.Idle)
            {
                if(basket.laundryState == _machine.laundryState)
                {
                    // �̹� ���� �ܰ��� ��踦 ���������� ��� �ִ� ��踦 ����
                    if (machine != null)
                    {
                        _machine.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.Click];
                        _machine.audioSource.Play();
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
                        _machine.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.Click];
                        _machine.audioSource.Play();
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
                        machine.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.ClickError];
                        machine.audioSource.Play();
                        machine = null;
                        basket = null;
                    }
                    else
                    {
                        basket.OnSelect(false);
                        basket.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.ClickError];
                        basket.audioSource.Play();
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
                    machine.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.ClickError];
                    machine.audioSource.Play();
                    machine = null;
                    basket = null;
                }
                else
                {
                    basket.OnSelect(false);
                    basket.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.ClickError];
                    basket.audioSource.Play();
                    basket = null;
                }
            }
        }
    }

    // ���� �ٱ��� ����
    public void OnBasketSelect()
    {
        if (machine != null)
        {
            machine.GetComponent<BasketController>().OnSelect(false);
            machine.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.ClickError];
            machine.audioSource.Play();
            machine = null;
        }

        if (basket == null)
        {
            basket = hit.collider?.GetComponent<BasketController>();
            basket.OnSelect(true);
            basket.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.Click];
            basket.audioSource.Play();
        }
        else
        {
            basket.OnSelect(false);
            basket = hit.collider?.GetComponent<BasketController>();
            basket.OnSelect(true);
            basket.audioSource.clip = AudioManager.Instance.sfxClips[(int)SFXType.Click];
            basket.audioSource.Play();
        }
    }

    // ��� Ȯ��
    public bool OnMachineCheck(int _UID)
    {
        for(int i = 0; i < machines.Length; i++)
        {
            MachineController _machine = machines[i].GetComponent<MachineController>();

            if(_machine.currentBasket !=  null)
            {
                if (_machine.currentBasket.customerUID == _UID)
                {
                    GameObject _basket = _machine.currentBasket.gameObject;
                    _machine.Init();
                    Destroy(_basket);
                    return true;
                }
            }
        }
        return false;
    }
}