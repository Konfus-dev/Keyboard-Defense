using UnityEngine;

namespace KeyboardCats.Input
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] 
        private Camera mainCamera;
        [SerializeField]
        private Texture2D defaultCursor;
        [SerializeField]
        private Texture2D hoverCursor;
        [SerializeField]
        private Texture2D clickCursor;

        private void Start()
        {
            SetCursor(defaultCursor);
        }

        private void Update()
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition), out var hit) && hit.collider.CompareTag("Clickable"))
            {
                SetCursor(hoverCursor);
                if (UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetMouseButton(1))
                {
                    SetCursor(clickCursor);
                }
            }
            else SetCursor(defaultCursor);
        }

        private void SetCursor(Texture2D cursorTexture)
        {
            Cursor.SetCursor(cursorTexture, Vector2.one * cursorTexture.width / 2 , CursorMode.Auto);
        }
    }
}