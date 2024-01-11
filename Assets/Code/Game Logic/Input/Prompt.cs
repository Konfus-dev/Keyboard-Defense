using System;
using System.Linq;
using KeyboardCats.Data;
using KeyboardCats.Input;
using Konfus.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.UI
{
    public class Prompt : KeyboardListener
    {
        [Header("Events")]
        [Space]
        public PromptCompletedEvent promptCompleted;
        public PromptCompletedEvent promptCharacterCorrectlyTyped;
        public PromptCompletedEvent promptCharacterIncorrectlyTyped;

        [Header("Dependencies")]
        [SerializeField]
        private PromptUI promptUI;
        
        private PromptData _prompt;
        private string _remainingPromptText;

        public void Generate(PromptDifficulty difficulty)
        {
            // Register to position manager
            PromptPositionManager.Instance.Register(transform);
            
            // Generate prompt
            _prompt = PromptManager.Instance.GeneratePrompt(difficulty);
            _remainingPromptText = _prompt;
            promptUI.Initialize(_prompt);
        }

        public override void OnDestroy()
        {
            PromptPositionManager.Instance.UnRegister(transform);
            base.OnDestroy();
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
            promptUI.OnNextCharInPromptTyped();
            promptCharacterCorrectlyTyped.Invoke();
        }

        private void OnTypedWrongCharacter()
        {
            Reset();
            promptCharacterIncorrectlyTyped.Invoke();
        }

        private void Reset()
        {
            _remainingPromptText = _prompt;
            promptUI.Reset();
        }
    }
    
    [Serializable]
    public class PromptCompletedEvent : UnityEvent { }
}