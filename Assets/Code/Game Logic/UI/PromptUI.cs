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
        [SerializeField]
        private float characterSpawnDelay = 0.1f;
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
        private Color _originalColor;
        private TMP_Text[] _promptCharacters;
        private PromptData _prompt;
        private string _remainingPrompt;
        
        public void SetPrompt(PromptData prompt)
        {
            // Update prompt
            _prompt = prompt;
            _remainingPrompt = prompt;
            Generate();
        }

        public void OnNextCharInPromptTyped()
        {
            StartCoroutine(RemoveCharRoutine());
        }

        public void OnStartHover()
        {
            foreach (var tmpText in _promptCharacters)
            {
                UpdateTMPColor(tmpText, Color.cyan);
            }
        }
        
        public void OnStopHover()
        {
            foreach (var tmpText in _promptCharacters)
            {
                UpdateTMPColor(tmpText, _originalColor);
            }
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
        
        private void Generate()
        {
            StartCoroutine(GenerateRoutine());
        }

        private IEnumerator GenerateRoutine()
        {
            root.gameObject.SetActive(false);
            yield return new WaitForSeconds(characterSpawnDelay);
            root.gameObject.SetActive(true);
            
            // Remove any old text
            if (_promptCharacters != null)
            {
                foreach (var tmpText in _promptCharacters)
                {
                    Destroy(tmpText.gameObject);
                }
            }
            
            // Generate new text
            _promptCharacters = new TMP_Text[_remainingPrompt.Length];
            for (int charIndex = 0; charIndex < _remainingPrompt.Length; charIndex++)
            {
                var characterUI = Instantiate(characterPrefab, root);
                var promptCharUI = characterUI.GetComponent<TMP_Text>();
                promptCharUI.text = _remainingPrompt[charIndex].ToString();
                _promptCharacters[charIndex] = promptCharUI;
                characterUI.gameObject.SetActive(false);
            }

            foreach (var character in _promptCharacters)
            {
                yield return new WaitForSeconds(characterSpawnDelay);
                character.gameObject.SetActive(true);
            }
            
            //root.gameObject.SetActive(true);
        }

        private IEnumerator RemoveCharRoutine()
        {
            var typedChar = _prompt.Value.First();
            
            // Get typed char and turn it red
            // TODO: add moar juice!
            var typedCharVisual = _promptCharacters.First();
            UpdateTMPColor(typedCharVisual, Color.red);
            
            // Update remaining text
            _remainingPrompt = _remainingPrompt.Remove(0, 1);
            _promptCharacters = _promptCharacters.RemoveAt(0);
            
            // After a small delay remove typed character
            yield return new WaitForSeconds(0.1f);
            Destroy(typedCharVisual.gameObject);
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
    }
}