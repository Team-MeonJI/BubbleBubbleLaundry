using UnityEngine;
using Utils.EnumTypes;

public class BasketController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] basketSprites;
    public Sprite[] basketSelectSprites;

    public MachineType machineType = MachineType.Basket;
    public LaundryState laundryState = LaundryState.Idle;

    public int customerUID = 0;
    public int laundryCount = 0;
    public int laundryZoneIndex = 0;
    private int spriteIndex = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = Random.Range(0, basketSprites.Length);
        spriteRenderer.sprite = basketSprites[spriteIndex];
    }

    public void OnNextStep()
    {
        if(laundryState == LaundryState.Complete)
        {
            Debug.Log("::: »¡·¡ ¿Ï·á :::");
            return;
        }
        laundryState++;
    }
}