using UnityEngine;
using UnityEngine.EventSystems;

public class BasketController : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Point Click");
        transform.position = transform.position;
    }

    private void OnMouseDrag()
    {
        Debug.Log("Point Drag");
        transform.position += transform.position;
    }

    private void OnMouseUp()
    {
        
    }
}
