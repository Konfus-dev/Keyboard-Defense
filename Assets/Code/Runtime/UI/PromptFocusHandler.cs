using KeyboardDefense.Input;
using KeyboardDefense.Prompts;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    public class PromptFocusHandler : MonoBehaviour
    {
        private Prompt _prompt;
        private PromptTextBox _promptTextBox;
        private IPromptFocusManager _promptFocusManager;
        private MouseEventListener _mouseEventListener;

        public void FocusPrompt()
        {
            _promptFocusManager.Focus(_promptTextBox);
        }

        public void RequestFocusOnPrompt()
        {
            var priority = (_prompt.GetPrompt().Length - _prompt.GetNumberOfRemainingCharacters()) + _prompt.GetPrompt().Length;
            _promptFocusManager.RequestFocus(_promptTextBox, priority);
        }
        
        private void Awake()
        {
            _prompt = GetComponent<Prompt>();
            _promptTextBox = GetComponent<PromptTextBox>();
            _mouseEventListener = GetComponent<MouseEventListener>();
        }

        private void Start()
        {
            _prompt.promptCharacterIncorrectlyTyped.AddListener(OnCharacterTyped);
            _prompt.promptCharacterCorrectlyTyped.AddListener(OnCharacterTyped);
            _mouseEventListener.mouseDown.AddListener(OnClick);
            _promptFocusManager = ServiceProvider.Get<IPromptFocusManager>();
        }
        
        private void OnDisable()
        {
            _promptFocusManager?.ClearFocus(_promptTextBox);
        }

        private void OnClick()
        {
            FocusPrompt();
        }
        
        private void OnClickAway()
        {
            _promptFocusManager?.ClearFocus(_promptTextBox);
        }

        private void OnCharacterTyped()
        {
            FocusPrompt();
        }
    }
}