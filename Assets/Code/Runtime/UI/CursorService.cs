using System;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class CursorService : GameService<ICursorService>, ICursorService
    {
        [SerializeField]
        private Texture2D defaultCursor;
        [SerializeField]
        private Texture2D hoverCursor;
        [SerializeField]
        private Texture2D clickCursor;
        [SerializeField]
        private Texture2D textCursor;

        private CursorState _cursorState;
        private CursorState _lastCursorState;

        public CursorState GetCursorState() => _cursorState;
        public CursorState GetLastCursorState() => _lastCursorState;

        public void SetCursor(CursorState cursorState)
        {
            if (cursorState == _cursorState) return;
            
            _lastCursorState = _cursorState;
            _cursorState = cursorState;
            
            switch (cursorState)
            {
                case CursorState.Default:
                    SetCursor(defaultCursor);
                    break;
                case CursorState.Hover:
                    SetCursor(hoverCursor);
                    break;
                case CursorState.Click:
                    SetCursor(clickCursor);
                    break;
                case CursorState.Text:
                    SetCursor(textCursor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_cursorState), cursorState, $"{cursorState} is not yet supported!");
            }
        }

        public void HideCursor()
        {
            Cursor.visible = false;
        }

        private void Start()
        {
            SetCursor(defaultCursor);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0) 
                && _cursorState != CursorState.Text)
            {
                SetCursor(CursorState.Click);
            }
            if (UnityEngine.Input.GetMouseButtonUp(0) 
                && _cursorState != CursorState.Text)
            {
                SetCursor(_lastCursorState == CursorState.Click
                    ? CursorState.Default 
                    : _lastCursorState);
            }
        }
        
        private void SetCursor(Texture2D cursorTexture)
        {
            Cursor.visible = true;
            Cursor.SetCursor(cursorTexture, Vector2.one * cursorTexture.width / 2 , CursorMode.Auto);
        }
    }

    public enum CursorState
    {
        Default,
        Hover,
        Click, 
        Text
    }
}