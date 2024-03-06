using UnityEngine;

namespace KeyboardDefense.Player.Input
{
    [RequireComponent(typeof(MouseEventListener))]
    public class Clickable : MonoBehaviour
    {
        private MouseEventListener _mouseEventListener;
        
        private void Awake()
        {
            _mouseEventListener = GetComponent<MouseEventListener>();
        }

        private void Start()
        {
            _mouseEventListener.mouseEnter.AddListener(OnStartHover);
            _mouseEventListener.mouseExit.AddListener(OnStopHover);
            _mouseEventListener.mouseDown.AddListener(OnClick);
        }

        private void OnClick()
        {
            //PlayerHealthUI.Instance.CursorUI.SetCursor(CursorState.Click);
        }

        private void OnStartHover()
        {
            //PlayerHealthUI.Instance.CursorUI.SetCursor(CursorState.Hover);
        }

        private void OnStopHover()
        {
            //PlayerHealthUI.Instance.CursorUI.SetCursor(CursorState.Default);
        }
    }
}