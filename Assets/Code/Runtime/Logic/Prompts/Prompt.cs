using System;
using System.Linq;
using KeyboardDefense.Logic.Input;
using Konfus.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Logic.Prompts
{
    public class Prompt : KeyboardListener
    {
        [Header("Events")]
        [Space]
        public PromptEvent promptCompleted;
        public PromptEvent promptCharacterCorrectlyTyped;
        public PromptEvent promptCharacterIncorrectlyTyped;
        
        private string _prompt;
        private string _remainingPromptText;
        
        public void Set(string prompt)
        {
            _prompt = prompt;
            _remainingPromptText = _prompt;
        }

        protected override void OnKeyPressed(string key)
        {
            if (_remainingPromptText.IsNullOrEmpty()) return;
            
            // Process input...
            string firstNonTypedCharInPrompt = _remainingPromptText.First().ToString().ToLower();
            string keyPressed = key.ToLower().Replace("space", " ");
            if (firstNonTypedCharInPrompt == keyPressed)
            {
                // User typed the next char in the non typed part of the prompt
                OnNextCharInPromptTyped();
            
                // Did user successfully type the entire prompt?
                if (_remainingPromptText.IsNullOrEmpty()) OnSuccessfullyTypedPrompt();
            }
            else if (_remainingPromptText != _prompt)
            {
                // user made mistake... punish them!
                OnTypedWrongCharacter();
            }
        }
        
        private void OnSuccessfullyTypedPrompt()
        {
            promptCompleted.Invoke();
        }

        private void OnNextCharInPromptTyped()
        {
            _remainingPromptText = _remainingPromptText.Remove(0, 1);
            promptCharacterCorrectlyTyped.Invoke();
        }

        private void OnTypedWrongCharacter()
        {
            _remainingPromptText = _prompt;
            promptCharacterIncorrectlyTyped.Invoke();
        }
    }
    
    [Serializable]
    public class PromptEvent : UnityEvent { }
}