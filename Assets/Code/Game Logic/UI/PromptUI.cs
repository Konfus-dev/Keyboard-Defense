using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using KeyboardCats.Data;
using Konfus.Utility.Extensions;
using TMPro;
using UnityEngine;

namespace KeyboardCats.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(BoxCollider))]
    public class PromptUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] 
        private Color hoverColor = Color.cyan;
        [SerializeField] 
        private Color resetColor = Color.red;
        [SerializeField] 
        private Color typedColor = Color.blue;
        [SerializeField]
        private float typeEffectDelay = 0.1f;
        
        [Header("Dependencies")]
        [SerializeField]
        private GameObject characterPrefab;
        [SerializeField]
        private RectTransform root;
        [SerializeField] 
        private Transform anchor;
        [SerializeField] 
        private RectTransform promptTail;
        
        private BoxCollider _clickCollider;
        
        // TODO: convert this to a list of words, then we can support hovering over individual words. 
        // this means each word needs to be its own TMP_Text not each character, so when we hover
        // the word will have an underline (indicating its a hyperlink of sorts).
        private PromptData _prompt;
        private string _remainingPrompt;
        private TMP_Text[] _promptCharacters;
        private TMP_Text[] _typedPromptCharacters;
        private Color _currentColor;
        private Color _originalColor;

        public void Initialize(PromptData prompt)
        {
            StartCoroutine(InitializeRoutine(prompt));
        }

        public void Reset()
        {
            _remainingPrompt = _prompt;
            StartCoroutine(TypeEffectRoutine(true, typeEffectDelay/2, resetColor));
        }

        public void OnNextCharInPromptTyped()
        {
            StartCoroutine(RemoveCharRoutine());
        }

        public void OnStartHover()
        {
            SetColor(hoverColor);
        }
        
        public void OnStopHover()
        {
            SetColor(_originalColor);
        }

        public void OnClicked()
        {
            foreach (var word in _prompt.Words)
            {
                Task.Run(async () =>
                {
                    Debug.Log($"Looking up: {word}...");
                    var def = await WordDictionary.LookupAsync(word).ContinueOnAnyContext();
                    Debug.Log(def);
                }).FireAndForget();
            }
        }

        private void Awake()
        {
            _clickCollider = GetComponent<BoxCollider>();
            _promptCharacters = GetComponentsInChildren<TMP_Text>();
            _originalColor = _promptCharacters.First().color;
            _currentColor = _originalColor;
        }

        private void Update()
        {
            KeepInScreenBounds();
            UpdateTail();
            UpdateClickCollider();
        }

        private void UpdateClickCollider()
        {
            _clickCollider.size = new Vector3(root.sizeDelta.x, root.sizeDelta.y, 1);
        }

        private bool IsOutOfScreenBounds(out Vector3 clampedPos)
        {
            Camera mainCamera = Camera.main;

            // Convert the world position to screen coordinates
            Vector3 screenPosLeft = mainCamera.WorldToScreenPoint(transform.position - (Vector3.right * 5));
            Vector3 screenPosRight = mainCamera.WorldToScreenPoint(transform.position + (Vector3.right * 5));

            // Get the screen boundaries
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            // Clamp the screen position to be within the screen boundaries
            float clampedX = Mathf.Clamp(screenPosLeft.x, 0, screenWidth);
            float clampedY = Mathf.Clamp(screenPosLeft.y, 0, screenHeight);


            // Convert the clamped screen position back to world coordinates
            clampedPos = mainCamera.ScreenToWorldPoint(new Vector3(clampedX, clampedY, screenPosLeft.z));

            // Check if the object is outside the screen boundaries
            return (screenPosLeft.x < 0 || screenPosLeft.x > screenWidth || screenPosLeft.y < 0 || screenPosLeft.y > screenHeight) || 
                   (screenPosRight.x < 0 || screenPosRight.x > screenWidth || screenPosRight.y < 0 || screenPosRight.y > screenHeight);
        }

        private void KeepInScreenBounds()
        {
            // Check if the GameObject is out of bounds
            if (IsOutOfScreenBounds(out var clampedPos))
            {
                transform.position = clampedPos;
            }
        }
        
        private void UpdateTail()
        {
            var scale = promptTail.localScale;
            promptTail.localScale =  new Vector3(scale.x, (root.transform.position - anchor.transform.position).magnitude, scale.z);
        }
        
        private void SetColor(Color color)
        {
            _currentColor = color;
            
            foreach (var tmpText in _promptCharacters)
            {
                UpdateTMPColor(tmpText, color);
            }
        }
        
        private void UpdateTMPColor(TMP_Text tmpText, Color color)
        {
            tmpText.color = color;
        }

        private void UpdateTMPColorViaRichFormatting(TMP_Text tmpText, Color color)
        {
            var hexColor = ColorUtility.ToHtmlStringRGB(color);
            tmpText.text = $"<color=\"#{hexColor}\">{tmpText.text}";
        }
        
        private IEnumerator InitializeRoutine(PromptData prompt)
        {
            // Spawn delay...
            root.gameObject.SetActive(false);
            yield return new WaitForSeconds(typeEffectDelay/2);
            root.gameObject.SetActive(true);
            
            // Remove any old text
            if (_promptCharacters != null)
            {
                foreach (var tmpText in _promptCharacters)
                {
                    Destroy(tmpText.gameObject);
                }
            }
            
            // Update prompt
            _prompt = prompt;
            _remainingPrompt = prompt;
            _promptCharacters = new TMP_Text[_remainingPrompt.Length];
            _typedPromptCharacters = new TMP_Text[_remainingPrompt.Length];
            yield return GenerateRoutine(true, false, typeEffectDelay, null);
        }
        
        private IEnumerator GenerateRoutine(bool playTypeEffect, bool reverseTypeEffect, float typeEffectSpeed, Color? typeEffectColor)
        {
            root.gameObject.SetActive(false);
            yield return new WaitForSeconds(typeEffectSpeed/2);
            root.gameObject.SetActive(true);
            
            // Generate new text
            for (int charIndex = 0; charIndex < _remainingPrompt.Length; charIndex++)
            {
                var characterUI = Instantiate(characterPrefab, root);
                var promptCharUI = characterUI.GetComponent<TMP_Text>();
                promptCharUI.text = _remainingPrompt[charIndex].ToString();
                _promptCharacters[charIndex] = promptCharUI;
                if (playTypeEffect) characterUI.gameObject.SetActive(false);
            }
            
            if (playTypeEffect)
            {
                yield return TypeEffectRoutine(reverseTypeEffect, typeEffectSpeed, typeEffectColor);
            }
        }

        private IEnumerator TypeEffectRoutine(bool reverseTypeEffect, float typeEffectSpeed, Color? typeEffectColor)
        {
            var charsToType = _promptCharacters.ToArray();
            if (reverseTypeEffect) charsToType = charsToType.Reverse().ToArray();
            foreach (var character in charsToType)
            {
                // Enable typed character and wait giving a typing effect
                if (character == null || character.gameObject.activeSelf) continue;
                UpdateTMPColor(character, _originalColor);
                character.gameObject.SetActive(true);
                yield return new WaitForSeconds(typeEffectSpeed/2);
                
                // Change color of typed char
                if (typeEffectColor == null) continue;
                UpdateTMPColor(character, typeEffectColor.Value);
                yield return new WaitForSeconds(typeEffectSpeed / 2);
                UpdateTMPColor(character, _currentColor);
            }
        }

        private IEnumerator RemoveCharRoutine()
        {
            // TODO: add moar juice!
            // Get typed char and turn it red
            var typedCharVisual = _promptCharacters.First(character => 
                character.text == _remainingPrompt[0].ToString());
            UpdateTMPColor(typedCharVisual, typedColor);
            
            // Update remaining text
            _remainingPrompt = _remainingPrompt.Remove(0, 1);
            
            // After a small delay remove typed character
            yield return new WaitForSeconds(0.1f);
            _typedPromptCharacters[_prompt.Value.IndexOf(typedCharVisual.text)] = typedCharVisual;
            typedCharVisual.gameObject.SetActive(false);
        }
    }
}