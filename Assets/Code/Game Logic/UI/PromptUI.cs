using System;
using System.Linq;
using KeyboardCats.Input;
using KeyboardCats.Prompts;
using Konfus.Utility.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace KeyboardCats.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TMP_Text))]
    public class PromptUI : KeyboardListener
    {
        public PromptCompletedEvent promptCompleted;
        
        // TODO: fx and such will be easier if this is an array of tmp text
        // with each element being a character in the prompt
        private TMP_Text _tmp;
        private RectTransform _rectTransform;
        
        private string _prompt;
        private string _remainingPromptText;

        public void Generate(PromptDifficulty difficulty)
        {
            // register to position manager
            _rectTransform = GetComponent<RectTransform>();
            PromptUIPositionManager.Instance.Register(_rectTransform);
            
            // Generate prompt and update ui visuals
            _prompt = PromptManager.Instance.GeneratePrompt(difficulty);
            _remainingPromptText = _prompt;
            UpdateVisual();
        }

        public override void OnDestroy()
        {
            PromptUIPositionManager.Instance.UnRegister(_rectTransform);
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
                // User successfully typed the entire prompt!
                if (_remainingPromptText.IsNullOrEmpty())
                {
                    OnSuccessfullyTypedPrompt();
                }
            }
            else
            {
                // user made mistake... punish them!
                OnTypedWrongCharacter();
            }
        }

        private void Awake()
        {
            _tmp = GetComponent<TMP_Text>();
        }
        
        private void OnSuccessfullyTypedPrompt()
        {
            promptCompleted.Invoke();
        }

        private void OnNextCharInPromptTyped()
        {
            _remainingPromptText = _remainingPromptText.Remove(0, 1);
            UpdateVisual();
        }

        private void OnTypedWrongCharacter()
        {
            Reset();
        }

        private void Reset()
        {
            _remainingPromptText = _prompt;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_tmp == null) return;
            _tmp.text = _remainingPromptText;
        }
    }
    
    [Serializable]
    public class PromptCompletedEvent : UnityEvent { }
}