using KeyboardDefense.Prompts;
using KeyboardDefense.Scenes;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [ExecuteInEditMode]
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
            var prompt = GetComponent<Prompt>();
            prompt.Set(text);
            GetComponent<PromptTextBox>().SetPrompt(text);
            GetComponent<Tooltip>().Set(tooltip);

            if (Application.isPlaying)
            {
                if (startFocused) GetComponent<PromptFocusHandler>().FocusPrompt();
                prompt.promptCompleted.AddListener(TransitionToScene);
            }
            if (objForTailToPointTo) GetComponentInChildren<PromptTail>().SetObjectToFollow(objForTailToPointTo);
        }

        private void TransitionToScene()
        {
            SceneManager.Instance.LoadScene(sceneToTransitionToOnTyped);
        }
    }
}