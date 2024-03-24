using KeyboardDefense.Player.Input;
using KeyboardDefense.Prompts;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class PromptFocusHandler : MonoBehaviour
    {
        private Prompt _prompt;
        private PromptUI _promptUI;
        private IPromptFocusManager _promptFocusManager;

        public void FocusPrompt()
        {
            _promptFocusManager.Focus(_promptUI);
        }

        public void RequestFocusOnPrompt()
        {
            var priority = (_prompt.GetPrompt().Length - _prompt.GetNumberOfRemainingCharacters()) + _prompt.GetPrompt().Length;
            _promptFocusManager.RequestFocus(_promptUI, priority);
        }
        
        private void Awake()
        {
            _prompt = GetComponent<Prompt>();
            _promptUI = GetComponent<PromptUI>();
            var mouseEventListener = GetComponent<MouseEventListener>();
            mouseEventListener.mouseDown.AddListener(OnClick);
            
            _prompt.promptCharacterIncorrectlyTyped.AddListener(OnCharacterTyped);
            _prompt.promptCharacterCorrectlyTyped.AddListener(OnCharacterTyped);
            _promptFocusManager = ServiceProvider.Instance.Get<IPromptFocusManager>();
        }
        
        private void OnDisable()
        {
            _promptFocusManager.ClearFocus(_promptUI);
        }

        private void OnClick()
        {
            FocusPrompt();
        }

        private void OnCharacterTyped()
        {
            RequestFocusOnPrompt();
        }
    }
}