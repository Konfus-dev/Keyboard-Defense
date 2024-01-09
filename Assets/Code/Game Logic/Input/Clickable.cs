using KeyboardCats.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnityEvent mouseDownEvent;
    public UnityEvent mouseEnterEvent;
    public UnityEvent mouseExitEvent;
    
    private void OnMouseEnter()
    {
        mouseEnterEvent.Invoke();
        CursorManager.Instance.SetCursor(CursorState.Hover);
    }

    private void OnMouseExit()
    {
        mouseExitEvent.Invoke();
        CursorManager.Instance.SetCursor(CursorState.Default);
    }

    private void OnMouseDown()
    {
        mouseDownEvent.Invoke();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }
}
