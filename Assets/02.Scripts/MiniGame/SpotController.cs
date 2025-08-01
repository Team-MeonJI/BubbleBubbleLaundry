using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpotController : MonoBehaviour, IPointerClickHandler
{
    private Image spotImage;
    private TextMeshProUGUI touchCountText;
    private MiniGame_1 miniGame1;

    private int quartiles;
    private int touchCount;
    private const int minTouchCount = 10;
    private const int maxTouchCount = 21;

    private void Awake()
    {
        spotImage = GetComponent<Image>();
        touchCountText = GetComponentInChildren<TextMeshProUGUI>();
        miniGame1 = GameObject.Find("MiniGameCanvas").transform.GetChild(0).GetComponent<MiniGame_1>();

        touchCount = Random.Range(minTouchCount, maxTouchCount);
        touchCountText.text = touchCount.ToString();
        quartiles = touchCount / 4;
    }

    public void OnTouch()
    {
        if(touchCount > 0)
        {
            touchCountText.text = touchCount.ToString();
            touchCount--;

            float _alpha = 255.0f;

            if (touchCount > quartiles * 3)
                _alpha = 255.0f;
            else if (touchCount > quartiles * 2)
                _alpha *= 0.75f;
            else if (touchCount > quartiles * 1)
                _alpha *= 0.5f;
            else
                _alpha *= 0.25f;

            spotImage.color = new Color(spotImage.color.r, spotImage.color.g, spotImage.color.b, _alpha / 255.0f);
        }
        else
        {
            touchCount = 0;
            touchCountText.text = touchCount.ToString();
            spotImage.color = new Color(spotImage.color.r, spotImage.color.g, spotImage.color.b, 0.0f);
            miniGame1.spots.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTouch();
    }
}