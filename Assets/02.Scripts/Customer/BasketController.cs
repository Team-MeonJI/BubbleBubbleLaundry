using UnityEngine;

public class BasketController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] basketSprites;
    public Sprite[] basketSelectSprites;

    public int customerUID = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = basketSprites[Random.Range(0, basketSprites.Length)];
    }

    // 빨래바구니 선택
    public void OnSelect()
    {
        
    }
}
