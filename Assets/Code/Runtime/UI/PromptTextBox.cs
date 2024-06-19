using UnityEngine;
using System.Collections;
using System.Linq;
using KeyboardDefense.Input;
using KeyboardDefense.Localization;
using KeyboardDefense.Prompts;
using KeyboardDefense.Services;
using TMPro;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(Tooltip))]
    [RequireComponent(typeof(MouseEventListener))]
    public class PromptTextBox : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private bool autoUpdateSize = true;
        [SerializeField] 
        private Color failedTextColor = Color.red;
        [SerializeField] 
        private Color typedTextColor = Color.blue;
        [SerializeField]
        private float typeEffectDelay = 0.05f;

        [Header("Dependencies")] 
        [SerializeField]
        private RectTransform root;
        [SerializeField]
        private GameObject highlight;
        [SerializeField]
        private GameObject textCursor;
        [SerializeField] 
        private TMP_Text promptText;
        
        private string _prompt;
        private string _currentText;
        private string _typedText;
        
        private FontStyles _originalStyle;
        private Color _originalColor;
        
        private Tooltip _tooltip;
        private MouseEventListener _mouseEventListener;

        public void Focus()
        {
            // Highlight and set cursor active
            highlight.SetActive(true);
            textCursor.SetActive(true);
        }

        public void Unfocus()
        {
            // Clear highlight and hide wcursor
            highlight.SetActive(false);
            textCursor.SetActive(false);
        }
        
        public void SetPrompt(string prompt)
        {
            _typedText = string.Empty;
            _prompt = prompt;
            _currentText = prompt;
            promptText.text = prompt;
            
            UpdateText();
            //if (!_isMenuPrompt) StartCoroutine(TypeOutPromptRoutine());
        }
        
        public void SetPrompt(PromptData promptData)
        {
            var promptTooltipHeader = promptData.Locale.GetCultureInfo().TextInfo.ToTitleCase(promptData.Word);
            _tooltip.Set(promptData.Definition, $"{promptTooltipHeader} Definition:");
            SetPrompt(promptData.Word);
        }

        public void SetSize(float size)
        {
            if (size <= 4)
            {
                root.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * 32);
            }
            else if (size <= 6)
            {
                root.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * 28);
            }
            else
            {
                root.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * 24);
            }
        }

        public void OnNextCharacterIncorrectlyTyped()
        {
            StartCoroutine(OnFailedToTypePromptRoutine());
        }
        
        public void OnNextCharInPromptCorrectlyTyped()
        {
            // Keep track of what has been typed
            _typedText += _currentText.FirstOrDefault();
            _currentText = _currentText.Remove(startIndex: 0, count: 1);
            
            // Update UI with new text
            UpdateText();
            
            // If we are paused from hovering, unpause
            Time.timeScale = 1;
        }

        public void OnPromptSuccessfullyTyped()
        {
            StartCoroutine(OnPromptSuccessfullyTypedRoutine());
        }

        private IEnumerator OnPromptSuccessfullyTypedRoutine()
        {
            // TODO: Show success effect
            yield return new WaitForSeconds(0.15f);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }

        private void Awake()
        {
            _tooltip = GetComponent<Tooltip>();
            _mouseEventListener = GetComponent<MouseEventListener>();
        }

        private void Start()
        {
            if (transform.parent == null)
            {
                var gameplayCanvas = ServiceProvider.Get<IGameplayCanvasProvider>();
                if (gameplayCanvas != null) transform.SetParent(gameplayCanvas.GameplayCanvas.transform);
            }
            _originalStyle = promptText.fontStyle;
            _originalColor = promptText.color;
            _mouseEventListener.mouseEnter.AddListener(OnStartHover);
            _mouseEventListener.mouseExit.AddListener(OnStopHover);
            _mouseEventListener.mouseDown.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _typedText = string.Empty;
            _currentText = string.Empty;
            promptText.text = string.Empty;
        }
        
        private void UpdateText()
        {
            var hexTextColor = ColorUtility.ToHtmlStringRGB(typedTextColor);
            promptText.text = $"<b><color=#{hexTextColor}>{_typedText}<b></color><i>{_currentText}</i>";
            if (autoUpdateSize) SetSize(_prompt.Length);
        }
        
        private void OnStartHover()
        {
            Time.timeScale = 0f;
        }

        private void OnStopHover()
        {
            Time.timeScale = 1;
        }

        private void OnClick()
        {
            //Time.timeScale = 1;
        }
        
        private IEnumerator OnFailedToTypePromptRoutine()
        {
            // Reset remaining text
            _typedText = string.Empty;
            _currentText = _prompt;
            promptText.text = _currentText;
            
            // Failed effect...
            promptText.color = failedTextColor;
            yield return new WaitForSeconds(typeEffectDelay/3);
            SetColor(_originalColor);
            yield return new WaitForSeconds(typeEffectDelay/3);
            SetColor(failedTextColor);
            yield return new WaitForSeconds(typeEffectDelay/3);
            SetColor(_originalColor);
        }
        
        private void SetColor(Color color)
        {
            promptText.color = color;
        }
    }
}