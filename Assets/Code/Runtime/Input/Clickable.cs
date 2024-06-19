using System;
using KeyboardDefense.Services;
using KeyboardDefense.UI;
using UnityEngine;

namespace KeyboardDefense.Input.Input
{
    [RequireComponent(typeof(MouseEventListener))]
    public class Clickable : MonoBehaviour
    {
        [SerializeField] 
        private bool isTextField;
        
        private MouseEventListener _mouseEventListener;
        private ICursorService _cursorService;

        private void Awake()
        {
            _mouseEventListener = GetComponent<MouseEventListener>();
        }

        private void Start()
        {
            _cursorService = ServiceProvider.Get<ICursorService>();
            _mouseEventListener.mouseEnter.AddListener(OnStartHover);
            _mouseEventListener.mouseExit.AddListener(OnStopHover);
            _mouseEventListener.mouseDown.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (isTextField) return;
            _cursorService.SetCursor(CursorState.Click);
        }

        private void OnStartHover()
        {
            _cursorService.SetCursor(isTextField ? CursorState.Text : CursorState.Hover);
        }

        private void OnStopHover()
        {
            _cursorService.SetCursor(CursorState.Default);
        }

        private void OnDestroy()
        {
            _cursorService.SetCursor(CursorState.Default);
        }
    }
}