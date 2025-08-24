using UnityEngine;

public class SpongeMoveController : MonoBehaviour
{
    private MiniGame_1 miniGame1;
    public RectTransform limitArea;
    private RectTransform spongeRect;
    private Animator animator;

    private bool isCursorHidden = false;

    void Awake()
    {
        miniGame1 = transform.parent.parent.parent.GetComponent<MiniGame_1>();
        spongeRect = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Sponge");
        }
        SetMovePosition();
    }

    // ��ġ ����
    public void SetMovePosition()
    {
        Vector2 localMousePos;
        Vector2 halfSize = limitArea.rect.size * 0.5f;

        // ���콺 ��ǥ�� limitArea ���� ���� ��ǥ�� ��ȯ
        RectTransformUtility.ScreenPointToLocalPointInRectangle(limitArea, Input.mousePosition, null, out localMousePos);

        if(limitArea.rect.Contains(localMousePos) && !miniGame1.isGameOver && !GameManager.Instance.isGameOver)
        {
            if (!isCursorHidden)
            {
                Cursor.visible = false;
                isCursorHidden = true;
            }
        }
        else
        {
            if (isCursorHidden)
            {
                Cursor.visible = true;
                isCursorHidden = false;
            }
        }

        // localMousePos�� ���� ������ ����� �ʵ��� Ŭ����
        localMousePos.x = Mathf.Clamp(localMousePos.x, -halfSize.x, halfSize.x);
        localMousePos.y = Mathf.Clamp(localMousePos.y, -halfSize.y, halfSize.y) - 60.0f;
        spongeRect.anchoredPosition = limitArea.anchoredPosition + localMousePos;
    }
}
