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
        
        private void Start()
        {
            GetComponent<PromptUI>().SetPrompt(text);
            GetComponent<Prompt>().Set(text);
            GetComponent<Tooltip>().Set(tooltip);
        }
    }
}