using System;
using Konfus.Systems.Cameras;
using Konfus.Utility.Design_Patterns;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.Input
{
    public class Player : Singleton<Player>
    {
        public KeyPressedEvent keyPressed;
        
        [SerializeField] 
        private RtsCamera rtsCamera;
        
        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            // Process move input
            float moveX = UnityEngine.Input.GetAxis("Horizontal");
            float moveY =  UnityEngine.Input.GetAxis("Vertical");
            OnMoveInput(moveX, moveY);
            
            // Process mouse move input
            Vector3 mousePos = UnityEngine.Input.mousePosition;
            OnMouseInput(mousePos.x, mousePos.y);
            
            // Process mouse click input
            bool rightClick =  UnityEngine.Input.GetMouseButton(0);
            bool leftClick =  UnityEngine.Input.GetMouseButton(1);
            if (rightClick || leftClick)
            {
                float mouseXDelta =  UnityEngine.Input.GetAxis("Mouse X");
                float mouseYDelta =  UnityEngine.Input.GetAxis("Mouse Y");
                OnClick(mouseXDelta, mouseYDelta);
            }
            else OnNoClick(mousePos);
            
            // Process zoom input
            float zoom =  UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            OnZoomInput(zoom);
            
            // Process keyboard input
            if (UnityEngine.Input.anyKeyDown) OnKeyPressed();
        }

        private void OnHover()
        {
            //threeDCursor.SetState("Hover");
        }

        private void OnMouseInput(float mouseX, float mouseY)
        {
            //threeDCursor.OnMouseInput(new Vector2(mouseX, mouseY));
        }
        
        private void OnClick(float mouseXDelta, float mouseYDelta)
        {
            //threeDCursor.SetState("Click");
            rtsCamera.OnRotateInput(new Vector2(mouseXDelta, mouseYDelta).normalized);
        }
        
        private void OnNoClick(Vector3 mousePos)
        {
            bool overInteractable = Physics.Raycast(
                ray: rtsCamera.Camera.ScreenPointToRay(mousePos),
                layerMask: LayerMask.GetMask("Interactable"), 
                maxDistance: 10f);
            if (overInteractable) OnHover();
            //else threeDCursor.SetState("Idle");
            rtsCamera.OnRotateInput(Vector2.zero);
        }

        private void OnMoveInput(float moveX, float moveY)
        {
            rtsCamera.OnMoveInput(new Vector2(moveX, moveY).normalized);
        }

        private void OnZoomInput(float zoom)
        {
            rtsCamera.OnZoomInput(zoom);
        }
        
        private void OnKeyPressed()
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (!UnityEngine.Input.GetKeyDown(keyCode)) continue;
                if (keyCode.ToString().Contains("Mouse")) continue;
                
                keyPressed.Invoke(keyCode.ToString());
            }
        }

        /*public void OnKeyPressed(InputAction.CallbackContext context)
        {
            // BUG: this is skipping key presses when typing fast...
            //Debug.Log($"Received input {context}");
            //if (!context.performed) return;
            
            var keyboard = (UnityEngine.InputSystem.Keyboard)context.control.parent;
            var pressedKeys = keyboard.allKeys.Where(key => key.isPressed);
            foreach (var key in pressedKeys)
            {
                Debug.Log($"{key} pressed");
                keyPressed.Invoke(key.name);
            }
        }*/
    }
    
    [Serializable]
    public class KeyPressedEvent : UnityEvent<string> { }
}
