using System;
using System.Collections.Generic;
using System.Linq;
using KeyboardDefense.Characters;
using KeyboardDefense.Services;
using Konfus.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Input
{
    public class Player : Character, IPlayer
    {
        public KeyPressedEvent KeyPressed { get; } = new();
        public HealthChangedEvent HealthChanged { get; } = new();
        
        private IGameStateService _gameStateService;
        
        public void Register()
        {
            ServiceProvider.Register<IPlayer>(this);
        }
        
        public void Unregister()
        {
            ServiceProvider.Unregister<IPlayer>();
        }
        
        public new void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            HealthChanged.Invoke(GetCurrentHealth(), GetStats().Health);
        }

        protected override void OnDie()
        {
            _gameStateService.GameState = IGameStateService.State.GameOver;
        }

        private void Start()
        {
            _gameStateService = ServiceProvider.Get<IGameStateService>();
        }

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
            const string MOUSE_KEY_PREFIX = "Mouse";
            if (!UnityEngine.Input.GetKeyDown(keyCode)) return true;
            if (keyCode.ToString().Contains(MOUSE_KEY_PREFIX)) return true;
            if (keyCode.Equals(KeyCode.LeftShift) || keyCode.Equals(KeyCode.RightShift)) return true;
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
            else if (keyCode == KeyCode.Quote) specialCharacter = "\'";
            else if (keyCode == KeyCode.Period) specialCharacter = ".";
            
            return !specialCharacter.IsNullOrEmpty();
        }

        private IEnumerable<string> GetTypedCharacters()
        {
            var shiftPressed = UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);
            var pressedKeys = new List<KeyCode>();
            var typedCharacters = new List<string>();
            
            // Get all the pressed keys
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
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
            if (typedCharacters.Contains("Escape"))
            {
                if (_gameStateService.GameState == IGameStateService.State.Playing) _gameStateService.PauseGame();
                else _gameStateService.ResumeGame();
            }
            foreach (var typedCharacter in typedCharacters) KeyPressed.Invoke(typedCharacter);
        }
    }
    
    // Event for when a key is pressed, passing in the key name
    public class KeyPressedEvent : UnityEvent<string> { }
    
    // Event for when health changes, passing in the current heath and max health in that order
    public class HealthChangedEvent : UnityEvent<int, int> { }
}
