using System;
using System.Linq;
using KeyboardDefense.Player.Input;
using Konfus.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardDefense.Prompts
{
    public class Prompt : KeyboardListener
    {
        [Header("Events")]
        [Space]
        public PromptEvent promptCompleted;
        public PromptEvent promptCharacterCorrectlyTyped;
        public PromptEvent promptCharacterIncorrectlyTyped;
        
        private string _text;
        private string _remainingPromptText;
        
        public void Set(string promptText)
        {
            _text = promptText;
            _remainingPromptText = _text;
        }
        
        public void Set(PromptData promptData)
        {
            _text = promptData.Word;
            _remainingPromptText = _text;
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
            else if (_remainingPromptText != _text)
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
            _remainingPromptText = _text;
            promptCharacterIncorrectlyTyped.Invoke();
        }
    }
    
    [Serializable]
    public class PromptEvent : UnityEvent { }
}