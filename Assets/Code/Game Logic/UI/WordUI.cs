using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using KeyboardCats.Data;
using Konfus.Utility.Extensions;
using TMPro;

namespace KeyboardCats.UI
{
    [RequireComponent(typeof(BoxCollider))]
    public class WordUI : MonoBehaviour
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
        private TMP_Text wordText;
        
        
        private BoxCollider _clickCollider;
        private DefinitionUI _definitionUI;
        private MouseEventListener _eventListener;
        
        private string _currentText;
        private string _typedText;
        private WordData _wordData;
        private FontStyles _originalStyle;
        private Color _originalColor;

        public IEnumerator InitializeRoutine(WordData word, MouseEventListener mouseEventListener, DefinitionUI definitionUI)
        {
            wordText.text = string.Empty;
            _wordData = word;
            _currentText = word;
            
            _eventListener = mouseEventListener;
            _definitionUI = definitionUI;
            SubToMouseEvents();
            
            // Type effect
            var charsToType = _wordData.ToString().ToArray();
            for (var charIndex = 0; charIndex < charsToType.Length; charIndex++)
            {
                UpdateBounds(charIndex);
                yield return new WaitForSeconds(typeEffectDelay);
                wordText.text += charsToType[charIndex];
            }
        }

        public IEnumerator OnFailedToTypeWordRoutine()
        {
            // Reset remaining text
            _typedText = string.Empty;
            _currentText = _wordData;
            wordText.text = _currentText;
            
            // Failed effect...
            wordText.color = failedColor;
            yield return new WaitForSeconds(typeEffectDelay/3);
            wordText.color = _originalColor;
            yield return new WaitForSeconds(typeEffectDelay/3);
            wordText.color = failedColor;
            yield return new WaitForSeconds(typeEffectDelay/3);
            wordText.color = _originalColor;
        }

        public void OnNextCharInWordTyped()
        {
            _typedText += _currentText.FirstOrDefault();
            _currentText = _currentText.Remove(startIndex: 0, count: 1);
            var hexColor = ColorUtility.ToHtmlStringRGB(typedColor);
            wordText.text = $"<color=#{hexColor}>{_typedText}</color>{_currentText}";
        }

        private void OnStartHover(GameObject go)
        {
            if (go != this) return;
            SetColor(hoverColor);
            SetStyle(FontStyles.Underline);
        }
        
        private void OnStopHover()
        {
            SetColor(_originalColor);
            SetStyle(_originalStyle);
        }

        private void OnClicked(GameObject go)
        {
            if (go != this) return;
            if (_definitionUI.IsOpen()) return;
            _definitionUI.Open();
            _definitionUI.Lookup(_wordData);
        }
        
        private void Awake()
        {
            _clickCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            _originalStyle = wordText.fontStyle;
            _originalColor = wordText.color;
        }

        private void OnDestroy()
        {
            UnSubToMouseEvents();
        }

        private void SubToMouseEvents()
        {
            _eventListener.mouseDown.AddListener(OnClicked);
            _eventListener.mouseEnter.AddListener(OnStartHover);
            _eventListener.mouseExit.AddListener(OnStopHover);
        }
        
        private void UnSubToMouseEvents()
        {
            if (_eventListener)
            {
                _eventListener.mouseDown.RemoveListener(OnClicked);
                _eventListener.mouseEnter.RemoveListener(OnStartHover);
                _eventListener.mouseExit.RemoveListener(OnStopHover);
            }
        }

        private void UpdateBounds(float size)
        {
            root.sizeDelta = new Vector2(size, 2);
            _clickCollider.size = new Vector3(size, 2, 1);
        }
        
        private void SetColor(Color color)
        {
            wordText.color = color;
        }

        private void SetStyle(FontStyles styles)
        {
            wordText.fontStyle = styles;
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