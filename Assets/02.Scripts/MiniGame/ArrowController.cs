using UnityEngine;
using UnityEngine.UI;
using Utils.EnumTypes;

public class ArrowController : MonoBehaviour
{
    private Image image;
    public ArrowType arrowType;

    public Sprite[] arrowSprites;

    private void Awake()
    {
        image = GetComponent<Image>();
        arrowType = (ArrowType)Random.Range(0, 4);
        image.sprite = arrowSprites[(int)arrowType];
    }
}
