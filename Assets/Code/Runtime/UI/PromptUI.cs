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
        private Color hoverColor = Color.cyan;
        [SerializeField] 
        private Color failedColor = Color.red;
        [SerializeField] 
        private Color typedColor = Color.blue;
        [SerializeField]
        private float typeEffectDelay = 0.1f;

        [Header("Dependencies")] 
        [SerializeField]
        private RectTransform root;
        [SerializeField] 
        private TMP_Text promptText;
        
        private string _prompt;
        private string _currentText;
        private string _typedText;
        
        private FontStyles _originalStyle;
        private Color _originalColor;
        
        private Tooltip _tooltip;
        private MouseEventListener _mouseEventListenter;

        public void SetPrompt(string prompt)
        {
            _typedText = string.Empty;
            _prompt = prompt;
            _currentText = prompt;
            StartCoroutine(TypeOutPromptRoutine());
        }
        
        public void SetPrompt(PromptData promptData)
        {
            _typedText = string.Empty;
            _prompt = promptData.Word;
            _currentText = promptData.Word;
            _tooltip.Set(promptData.Definition, $"{promptData.Locale.GetCultureInfo().TextInfo.ToTitleCase(_prompt)} Definition:");
            StartCoroutine(TypeOutPromptRoutine());
        }

        public void OnNextCharacterIncorrectlyTyped()
        {
            StartCoroutine(OnFailedToTypePromptRoutine());
        }
        
        public void OnNextCharInPromptTyped()
        {
            _typedText += _currentText.FirstOrDefault();
            _currentText = _currentText.Remove(startIndex: 0, count: 1);
            var hexColor = ColorUtility.ToHtmlStringRGB(typedColor);
            promptText.text = $"<color=#{hexColor}>{_typedText}</color>{_currentText}";
        }

        public void OnPromptSuccessfullyTyped()
        {
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

        // TODO: this isn't working!!! Get it to work...
        private void OnStartHover()
        {
            SetColor(hoverColor);
            SetStyle(FontStyles.Underline);
        }

        private void OnStopHover()
        {
            SetColor(_originalColor);
            SetStyle(_originalStyle);
        }

        private void OnClick()
        {
            // TODO: create tooltip system, then use it to open a definition tooltip here!
        }

        private IEnumerator TypeOutPromptRoutine()
        {
            // Type effect
            promptText.text = string.Empty;
            var charsToType = _prompt.ToArray();
            for (var charIndex = 0; charIndex < charsToType.Length; charIndex++)
            {
                UpdateSize(charIndex + 1);
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
            promptText.color = failedColor;
            yield return new WaitForSeconds(typeEffectDelay/3);
            SetColor(_originalColor);
            yield return new WaitForSeconds(typeEffectDelay/3);
            SetColor(failedColor);
            yield return new WaitForSeconds(typeEffectDelay/3);
            SetColor(_originalColor);
        }

        private void UpdateSize(float size)
        {
            root.sizeDelta = new Vector2(size * 32, 60);
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