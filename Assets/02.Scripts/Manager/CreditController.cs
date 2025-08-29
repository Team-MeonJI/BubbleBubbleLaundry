using UnityEngine;
using DG.Tweening;

public class CreditController : MonoBehaviour
{
    private GameObject mainCanvas;
    private GameObject creditObject;
    private RectTransform creditTransform;

    Tween moveTween;
    private Vector2 originPos;
    private float targetY = 450.0f;
    private float duration = 60.0f;

    private void Awake()
    {
        mainCanvas = GameObject.Find("Canvas");
        creditObject = mainCanvas.transform.GetChild(3).gameObject;
        creditTransform = creditObject.transform.GetChild(0).GetComponent<RectTransform>();

        originPos = creditTransform.anchoredPosition;
    }

    public void OnCredit()
    {
        creditObject.SetActive(true);
        Vector2 endPos = new Vector2(originPos.x, targetY);
        moveTween = creditTransform.DOAnchorPos(endPos, duration).OnComplete(() => { Debug.Log(":: Credit End ::"); });
    }

    public void OnCloseCredit()
    {
        moveTween.Kill();
        creditObject.SetActive(false);
        creditTransform.anchoredPosition = originPos;
    }
}