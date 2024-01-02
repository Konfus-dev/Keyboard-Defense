using System;
using Konfus.Utility.Design_Patterns;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.Input
{
    public class Player : Singleton<Player>
    {
        public KeyPressedEvent keyPressed;
        
        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (UnityEngine.Input.anyKeyDown) OnKeyPressed();
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
