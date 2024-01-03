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
        public PromptCompletedEvent promptCompleted;

        [SerializeField]
        private PromptUI promptUI;
        
        private string _prompt;
        private string _remainingPromptText;

        public void Generate(PromptDifficulty difficulty)
        {
            // Register to position manager
            PromptPositionManager.Instance.Register(transform);
            
            // Generate prompt
            _prompt = PromptManager.Instance.GeneratePrompt(difficulty);
            _remainingPromptText = _prompt;
            promptUI.SetPrompt(_remainingPromptText);
        }

        public override void OnDestroy()
        {
            PromptPositionManager.Instance.UnRegister(transform);
            base.OnDestroy();
        }

        protected override void OnKeyPressed(string key)
        {
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
            else
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
        }

        private void OnTypedWrongCharacter()
        {
            Reset();
        }

        private void Reset()
        {
            _remainingPromptText = _prompt;
            promptUI.SetPrompt(_remainingPromptText);
        }
    }
    
    [Serializable]
    public class PromptCompletedEvent : UnityEvent { }
}