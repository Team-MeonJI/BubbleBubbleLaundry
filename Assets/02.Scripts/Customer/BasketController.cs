using System.Linq;
using UnityEngine;
using Utils.EnumTypes;
using System.Collections.Generic;

public class BasketController : MonoBehaviour
{
    public AudioSource audioSource;
    public SpriteRenderer spriteRenderer;
    public Sprite[] basketSprites;
    public Sprite[] basketSelectSprites;

    public GameObject laundryPrefab;
    public Sprite[] laundrySprites;
    private List<Transform> completeZone;

    public MachineType machineType = MachineType.Basket;
    public LaundryState laundryState = LaundryState.Idle;

    public int customerUID = 0;
    public int laundryCount = 0;
    public int laundryZoneIndex = 0;
    public int spriteIndex = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = Random.Range(0, basketSprites.Length);
        spriteRenderer.sprite = basketSprites[spriteIndex];
        completeZone = GameObject.Find("CompleteZone").transform.GetChild(1).GetComponentsInChildren<Transform>().ToList();
        completeZone.RemoveAt(0);
        completeZone = Enumerable.Reverse(completeZone).ToList();
    }

    public void OnNextStep()
    {
        laundryState++;
    }

    public void OnComplete()
    {
        Debug.Log("::: 빨래 완료 :::");
        GameObject _laundry = Instantiate(laundryPrefab, completeZone[laundryZoneIndex].position, Quaternion.identity);
        SpriteRenderer[] _laundrySprite = _laundry.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < _laundrySprite.Length; i++)
        {
            if(i < laundryCount)
            {
                _laundrySprite[i].sprite = laundrySprites[Random.Range(0, laundrySprites.Length)];
                _laundrySprite[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                _laundrySprite[i].color = new Color(1, 1, 1, 0);
            }
        }

        if (CustomerManager.Instance.completeZoneCustomers[laundryZoneIndex] != null)
        {
            if (customerUID == CustomerManager.Instance.completeZoneCustomers[laundryZoneIndex].customerUID)
            {
                if (CustomerManager.Instance.completeZoneCustomers[laundryZoneIndex].state != CustomerState.Angry)
                {
                    UIManager.Instance.ChangeCompleteText();
                    GameManager.Instance.ReputationHandler(GameManager.Instance.reputeAdd);
                    GameManager.Instance.MoneyHandler(laundryCount * 100);
                    CustomerManager.Instance.CoroutineHandler(laundryZoneIndex, _laundry, gameObject, 1);
                }
            }
        }
    }

    // 바구니 선택
    public void OnSelect(bool isActive)
    {
        if(!isActive)
            spriteRenderer.sprite = basketSprites[spriteIndex];
        else
            spriteRenderer.sprite = basketSelectSprites[spriteIndex];
    }
}