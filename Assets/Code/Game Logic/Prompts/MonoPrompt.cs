using System;
using System.Linq;
using KeyboardCats.Input;
using Konfus.Utility.Extensions;
using TMPro;
using UnityEngine;

namespace KeyboardCats.Prompts
{
    [RequireComponent(typeof(TMP_Text))]
    public class MonoPrompt : KeyboardListener
    {
        // TODO: fx and such will be easier if this is an array of tmp text
        // with each element being a character in the prompt
        private TMP_Text _tmp;
        private string _prompt;
        private string _remainingPromptText;

        private void Awake()
        {
            _tmp = GetComponent<TMP_Text>();
        }

        private new void Start()
        {
            _prompt = PromptManager.Instance.GeneratePrompt();
            _remainingPromptText = _prompt;
            UpdateVisual();
            base.Start();
        }
        
        protected override void OnKeyPressed(string key)
        {
            //Debug.Log($"{name} processed: {key}");
            
            // User successfully typed the entire prompt!
            if (_remainingPromptText.IsNullOrEmpty())
            {
                OnSuccessfullyTypedPrompt();
                return;
            }
            
            // Process input...
            string firstNonTypedCharInPrompt = _remainingPromptText.First().ToString().ToLower();
            string keyPressed = key.ToLower().Replace("space", " ");
            if (firstNonTypedCharInPrompt == keyPressed)
            {
                // User typed the next char in the non typed part of the prompt
                OnNextCharInPromptTyped();
            }
            else
            {
                // user made mistake... punish them!
                OnTypedWrongCharacter();
            }
        }

        private void OnSuccessfullyTypedPrompt()
        {
            // TODO: need to figure out what we do when prompt is successfully typed
            Debug.Log($"{name}: successfully typed prompt! Yay!");
        }

        private void OnNextCharInPromptTyped()
        {
            _remainingPromptText = _remainingPromptText.Remove(0, 1);
            UpdateVisual();
        }

        private void OnTypedWrongCharacter()
        {
            Debug.Log($"{name}: wrong character typed!");
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
}