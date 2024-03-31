using KeyboardDefense.Prompts;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(Prompt))]
    [RequireComponent(typeof(Tooltip))]
    [RequireComponent(typeof(PromptUI))]
    public class MenuPromptUI : MonoBehaviour
    {
        [SerializeField]
        private string text;
        [SerializeField, Multiline]
        private string tooltip;
        [SerializeField] 
        private bool startFocused;
        
        public string Text => text;
        public string Tooltip => tooltip;

        private void Start()
        {
            GetComponent<PromptUI>().SetPrompt(text);
            GetComponent<Prompt>().Set(text);
            GetComponent<Tooltip>().Set(tooltip);
            
            if (startFocused) GetComponent<PromptFocusHandler>().FocusPrompt();
        }
    }
}