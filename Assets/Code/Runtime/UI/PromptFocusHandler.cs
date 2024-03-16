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

        private void Awake()
        {
            _prompt = GetComponent<Prompt>();
            _prompt.promptCharacterIncorrectlyTyped.AddListener(OnCharacterTyped);
            _prompt.promptCharacterCorrectlyTyped.AddListener(OnCharacterTyped);
            _promptUI = GetComponent<PromptUI>();
            _promptFocusManager = ServiceProvider.Instance.Get<IPromptFocusManager>();
        }
        
        private void OnEnable()
        {
            _promptFocusManager.ClearFocus(_promptUI);
        }

        private void OnCharacterTyped()
        {
            var priority = (_prompt.GetPrompt().Length - _prompt.GetNumberOfRemainingCharacters()) + _prompt.GetPrompt().Length;
            _promptFocusManager.RequestFocus(_promptUI, priority);
        }
    }
}