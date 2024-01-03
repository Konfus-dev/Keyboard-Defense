using System;
using System.Collections.Generic;
using Konfus.Utility.Design_Patterns;
using Konfus.Utility.Extensions;
using PlasticGui;
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

        private bool FilterKeyPress(KeyCode keyCode)
        {
            if (keyCode.Equals(KeyCode.LeftShift) || keyCode.Equals(KeyCode.RightShift)) return true;
            if (!UnityEngine.Input.GetKeyDown(keyCode)) return true;
            if (keyCode.ToString().Contains("Mouse")) return true;
            
            return false;
        }

        private bool TryParseSpecialCharacters(KeyCode keyCode, bool shiftPressed, out string specialCharacter)
        {
            specialCharacter = string.Empty;
            
            if (shiftPressed && keyCode == KeyCode.Alpha1) specialCharacter = "!";
            else if (shiftPressed && keyCode == KeyCode.Alpha2) specialCharacter = "@";
            else if (shiftPressed && keyCode == KeyCode.Alpha3) specialCharacter = "&";
            else if (shiftPressed && keyCode == KeyCode.Alpha4) specialCharacter = "$";
            else if (shiftPressed && keyCode == KeyCode.Alpha5) specialCharacter = "%";
            else if (shiftPressed && keyCode == KeyCode.Alpha7) specialCharacter = "#";
            else if (shiftPressed && keyCode == KeyCode.Slash) specialCharacter = "?";
            else if (shiftPressed && keyCode == KeyCode.Quote) specialCharacter = "\"";
            else if (keyCode == KeyCode.Quote) specialCharacter = "'";
            else if (keyCode == KeyCode.Period) specialCharacter = ".";
            
            return !specialCharacter.IsNullOrEmpty();
        }

        private IEnumerable<string> GetTypedCharacters()
        {
            var shiftPressed = false;
            var pressedKeys = new List<KeyCode>();
            var typedCharacters = new List<string>();
            
            // Get all the pressed keys
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (keyCode.Equals(KeyCode.LeftShift) || keyCode.Equals(KeyCode.RightShift)) shiftPressed = true;
                if (FilterKeyPress(keyCode)) continue;
                pressedKeys.Add(keyCode);
            }

            // Convert to the typed characters
            foreach (var keyCode in pressedKeys)
            {
                typedCharacters.Add(TryParseSpecialCharacters(keyCode, shiftPressed, out var specialCharacter)
                    ? specialCharacter
                    : keyCode.ToString());
            }

            return typedCharacters;
        }
        
        private void OnKeyPressed()
        {
            var typedCharacters = GetTypedCharacters();
            foreach (var typedCharacter in typedCharacters) keyPressed.Invoke(typedCharacter);
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
