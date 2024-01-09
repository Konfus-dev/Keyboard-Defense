using KeyboardCats.Input;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CursorState _lastCursorState;
    
    private void OnMouseEnter()
    {
        CursorManager.Instance.SetCursor(CursorState.Hover);
    }
    
    private void OnMouseExit()
    {
        CursorManager.Instance.SetCursor(CursorState.Default);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit();
    }
}
