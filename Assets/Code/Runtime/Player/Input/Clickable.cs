using KeyboardDefense.Services;
using KeyboardDefense.UI;
using UnityEngine;

namespace KeyboardDefense.Player.Input
{
    [RequireComponent(typeof(MouseEventListener))]
    public class Clickable : MonoBehaviour
    {
        private MouseEventListener _mouseEventListener;
        private ICursorService _cursorService;

        private void Awake()
        {
            _mouseEventListener = GetComponent<MouseEventListener>();
        }

        private void Start()
        {
            _cursorService = ServiceProvider.Instance.Get<ICursorService>();
            _mouseEventListener.mouseEnter.AddListener(OnStartHover);
            _mouseEventListener.mouseExit.AddListener(OnStopHover);
            _mouseEventListener.mouseDown.AddListener(OnClick);
        }

        private void OnClick()
        {
            _cursorService.SetCursor(CursorState.Click);
        }

        private void OnStartHover()
        {
            _cursorService.SetCursor(CursorState.Hover);
        }

        private void OnStopHover()
        {
            _cursorService.SetCursor(CursorState.Default);
        }
    }
}