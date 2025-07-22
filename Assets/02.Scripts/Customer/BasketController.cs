using UnityEngine;

public class BasketController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] basketSprites;
    public Sprite[] basketSelectSprites;

    public int customerUID = 0;
    private int spriteIndex = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteIndex = Random.Range(0, basketSprites.Length);
        spriteRenderer.sprite = basketSprites[spriteIndex];
    }

    // 빨래바구니 선택
    public void OnSelect()
    {
        
    }
}