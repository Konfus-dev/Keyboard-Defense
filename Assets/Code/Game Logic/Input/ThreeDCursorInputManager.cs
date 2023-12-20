using Konfus.Systems.ThreeDCursor;
using UnityEngine;

namespace KeyboardCats.Input
{
    /// <summary>
    /// Example of how to use the 3d cursor, I don't recommend doing this exactly I would at least
    /// utilize the new input system instead of the old one like I'm doing here...
    /// </summary>
    public class ThreeDCursorInputManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private LayerMask layersToHover;
    
        [Header("Dependencies")]
        [SerializeField]
        private ThreeDCursor threeDCursor;
        [SerializeField] 
        private Camera mainCamera;

        private void Update()
        {
            Vector3 mousePos = UnityEngine.Input.mousePosition;
            
            if (UnityEngine.Input.GetMouseButton(0)) OnClick();
            else if (UnityEngine.Input.GetMouseButton(1)) OnZoom();
            else if (Physics.Raycast(ray: mainCamera.ScreenPointToRay(mousePos), layerMask: layersToHover, maxDistance: 10f))
            {
                OnHover();
            }
            else
            {
                threeDCursor.SetState("Idle");
            }

            threeDCursor.OnMouseInput(mousePos);
        }

        private void OnClick()
        {
            threeDCursor.SetState("Click");
        }

        private void OnHover()
        {
            threeDCursor.SetState("Hover");
        }

        private void OnZoom()
        {
            threeDCursor.SetState("Magnify");
        }
    }
}
