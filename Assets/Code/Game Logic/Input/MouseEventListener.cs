using KeyboardCats.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseEventListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public UnityEvent mouseDown;
    public UnityEvent mouseEnter;
    public UnityEvent mouseExit;
    
    private void OnMouseEnter()
    {
        mouseEnter.Invoke();
        CursorManager.Instance.SetCursor(CursorState.Hover);
    }

    private void OnMouseExit()
    {
        mouseExit.Invoke();
        CursorManager.Instance.SetCursor(CursorState.Default);
    }

    private void OnMouseDown()
    {
        mouseDown.Invoke();
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
