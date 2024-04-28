using System;
using KeyboardDefense.Prompts;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [ExecuteInEditMode]
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
        private GameObject objForTailToPointTo;
        [SerializeField] 
        private bool startFocused;
        
        public string Text => text;
        public string Tooltip => tooltip;

        private void Awake()
        {
            Initialize();
        }

        private void OnValidate()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            GetComponent<PromptUI>().SetPrompt(text);
            GetComponent<Prompt>().Set(text);
            GetComponent<Tooltip>().Set(tooltip);
            
            if (startFocused && Application.isPlaying) GetComponent<PromptFocusHandler>().FocusPrompt();
            if (objForTailToPointTo) GetComponentInChildren<PromptTail>().SetObjectToFollow(objForTailToPointTo);
        }
    }
}