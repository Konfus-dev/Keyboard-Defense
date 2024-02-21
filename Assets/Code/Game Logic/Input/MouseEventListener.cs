using KeyboardCats.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseEventListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public MouseEvent mouseDown;
    public MouseEvent mouseEnter;
    public UnityEvent mouseExit;

    private GameObject _focusedOn;
    
    private void OnMouseEnter()
    {
        if (_focusedOn == null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                _focusedOn = info.collider.gameObject;
            }
        }
        mouseEnter.Invoke(_focusedOn);
        CursorManager.Instance.SetCursor(CursorState.Hover);
        _focusedOn = null;
    }

    private void OnMouseExit()
    {
        mouseExit.Invoke();
        CursorManager.Instance.SetCursor(CursorState.Default);
    }

    private void OnMouseDown()
    {
        if (_focusedOn == null)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit info))
            {
                _focusedOn = info.collider.gameObject;
            }
        }
        mouseDown.Invoke(_focusedOn);
        _focusedOn = null;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _focusedOn = eventData.pointerEnter;
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _focusedOn = eventData.pointerClick;
        OnMouseDown();
    }
}

[System.Serializable]
public class MouseEvent : UnityEvent<GameObject> { }
