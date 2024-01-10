using System;
using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardCats.Input
{
    public class CursorManager : Singleton<CursorManager>
    {
        [SerializeField]
        private Texture2D defaultCursor;
        [SerializeField]
        private Texture2D hoverCursor;
        [SerializeField]
        private Texture2D clickCursor;

        private CursorState _cursorState;
        private CursorState _lastCursorState;
        
        private void Start()
        {
            SetCursor(defaultCursor);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                SetCursor(CursorState.Click);
            }
            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                SetCursor(_lastCursorState == CursorState.Click
                    ? CursorState.Default 
                    : _lastCursorState);
            }
        }

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
                default:
                    throw new ArgumentOutOfRangeException($"{cursorState} is not yet supported!", cursorState, null);
            }
        }
        
        private void SetCursor(Texture2D cursorTexture)
        {
            UnityEngine.Cursor.SetCursor(cursorTexture, Vector2.one * cursorTexture.width / 2 , CursorMode.Auto);
        }
    }

    public enum CursorState
    {
        Default,
        Hover,
        Click
    }
}