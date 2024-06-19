using System;
using KeyboardDefense.Prompts;
using KeyboardDefense.Scenes;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(Prompt))]
    [RequireComponent(typeof(Tooltip))]
    [RequireComponent(typeof(PromptTextBox))]
    public class MenuPrompt : MonoBehaviour
    {
        [SerializeField]
        private string text;
        [SerializeField, Multiline]
        private string tooltip;
        [SerializeField]
        private SceneInfo sceneToTransitionToOnTyped;
        [SerializeField]
        private GameObject objForTailToPointTo;
        [SerializeField] 
        private bool startFocused;

        private Prompt _prompt;
        private PromptTextBox _promptTextBox;
        private Tooltip _tooltip;
        private PromptFocusHandler _promptFocusHandler;
        private PromptTail _promptTail;
        private ISceneManager _sceneManager;

        private void Awake()
        {
            _prompt = GetComponent<Prompt>();
            _promptTextBox = GetComponent<PromptTextBox>();
            _tooltip = GetComponent<Tooltip>();
            _promptFocusHandler = GetComponent<PromptFocusHandler>();
            _promptTail = GetComponentInChildren<PromptTail>();
        }

        private void Start()
        {
            _prompt.Set(text);
            _tooltip.Set(tooltip);
            _promptTextBox.SetPrompt(text);
            if (_promptTail)
            {
                _promptTail.SetObjectToFollow(objForTailToPointTo);
            }
            
            _sceneManager = ServiceProvider.Get<ISceneManager>();
            if (startFocused) _promptFocusHandler.FocusPrompt();
            _prompt.promptCompleted.AddListener(TransitionToScene);
        }

        private void TransitionToScene()
        {
            _sceneManager.LoadScene(sceneToTransitionToOnTyped);
        }
    }
}