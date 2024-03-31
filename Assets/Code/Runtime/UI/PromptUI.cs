using UnityEngine;
using System.Collections;
using System.Linq;
using KeyboardDefense.Localization;
using KeyboardDefense.Player.Input;
using KeyboardDefense.Prompts;
using TMPro;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(Tooltip))]
    [RequireComponent(typeof(MouseEventListener))]
    public class PromptUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Color hoverTextColor = Color.cyan;
        [SerializeField] 
        private Color failedTextColor = Color.red;
        [SerializeField] 
        private Color typedTextColor = Color.blue;
        [SerializeField]
        private float typeEffectDelay = 0.1f;

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
        private MouseEventListener _mouseEventListenter;

        public void Focus()
        {
            // Highlight and set cursor active
            highlight.SetActive(true);
            textCursor.SetActive(true);
            
            // Move to front
            // TODO: fix this! Its not working and we need to move the thing we are focused on in front of all other UI elements!
            var position = transform.position;
            position = new Vector3(position.x, position.y, 10);
            transform.position = position;
        }

        public void Unfocus()
        {
            // Clear highlight and hide wcursor
            highlight.SetActive(false);
            textCursor.SetActive(false);
            
            // Move back
            var position = transform.position;
            position = new Vector3(position.x, position.y, 0);
            transform.position = position;
        }
        
        public void SetPrompt(string prompt)
        {
            _typedText = string.Empty;
            _prompt = prompt;
            _currentText = prompt;
            promptText.text = prompt;
            
            UpdateText();
            //StartCoroutine(TypeOutPromptRoutine());
        }
        
        public void SetPrompt(PromptData promptData)
        {
            var promptTooltipHeader = promptData.Locale.GetCultureInfo().TextInfo.ToTitleCase(promptData.Word);
            _tooltip.Set(promptData.Definition, $"{promptTooltipHeader} Definition:");
            SetPrompt(promptData.Word);
        }

        public void SetSize(float size)
        {
            root.sizeDelta = new Vector2(size * 32, 60);
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
        }

        public void OnPromptSuccessfullyTyped()
        {
            StartCoroutine(OnPromptSuccessfullyTypedRoutine());
        }

        private IEnumerator OnPromptSuccessfullyTypedRoutine()
        {
            // TODO: Show success effect
            yield return new WaitForSeconds(0.15f);
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _tooltip = GetComponent<Tooltip>();
            _mouseEventListenter = GetComponent<MouseEventListener>();
        }

        private void Start()
        {
            _originalStyle = promptText.fontStyle;
            _originalColor = promptText.color;
            /*_mouseEventListenter.mouseEnter.AddListener(OnStartHover);
            _mouseEventListenter.mouseExit.AddListener(OnStopHover);
            _mouseEventListenter.mouseDown.AddListener(OnClick);*/
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
        }
        
        private void OnStartHover()
        {
            SetColor(hoverTextColor);
            SetStyle(FontStyles.Underline);
        }

        private void OnStopHover()
        {
            SetColor(_originalColor);
            SetStyle(_originalStyle);
        }

        private void OnClick()
        {
            //Focus();
        }
        
        private IEnumerator TypeOutPromptRoutine()
        {
            // Type effect
            promptText.text = string.Empty;
            var charsToType = _prompt.ToArray();
            for (var charIndex = 0; charIndex < charsToType.Length; charIndex++)
            {
                SetSize(charIndex + 1);
                yield return new WaitForSeconds(typeEffectDelay);
                promptText.text += charsToType[charIndex];
            }
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

        private void SetStyle(FontStyles styles)
        {
            promptText.fontStyle = styles;
        }

        private void UpdateTMPColorViaRichFormatting(TMP_Text tmpText, string textToUpdate, Color color)
        {
        }
        
        private void RemoveTMPColorViaRichFormatting(TMP_Text tmpText, string textToUpdate, Color color)
        {
            var hexColor = ColorUtility.ToHtmlStringRGB(color);
            var text = tmpText.text;
            tmpText.text = text.Replace(
                oldValue: $"<color=#{hexColor}>{textToUpdate}</color>", 
                newValue: textToUpdate);
        }
    }
}