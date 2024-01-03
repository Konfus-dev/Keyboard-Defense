using System.Collections;
using System.Linq;
using Konfus.Utility.Extensions;
using TMPro;
using UnityEngine;

namespace KeyboardCats.UI
{
    public class PromptUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject characterPrefab;
        
        private TMP_Text[] _promptVisual;
        private string _prompt;
        
        public void SetPrompt(string prompt)
        {
            _prompt = prompt;
            Generate();
        }

        public void OnNextCharInPromptTyped()
        {
            StartCoroutine(RemoveRoutine());
        }

        private void Generate()
        {
            // Remove any old text
            if (_promptVisual != null)
            {
                foreach (var tmpText in _promptVisual)
                {
                    Destroy(tmpText.gameObject);
                }
            }
            
            // Generate new text
            _promptVisual = new TMP_Text[_prompt.Length];
            for (int charIndex = 0; charIndex < _prompt.Length; charIndex++)
            {
                var characterUI = Instantiate(characterPrefab, transform);
                var promptCharUI = characterUI.GetComponent<TMP_Text>();
                promptCharUI.text = _prompt[charIndex].ToString();
                _promptVisual[charIndex] = promptCharUI;
            }
        }

        private IEnumerator RemoveRoutine()
        {
            var typedChar = _prompt.First();
            
            // Get typed char and turn it red
            // TODO: add moar juice!
            var typedCharVisual = _promptVisual.First();
            typedCharVisual.text = $"<color=red>{typedChar}";
            
            // Update remaining text
            var remainingText = _prompt.Remove(0, 1);
            _prompt = remainingText;
            _promptVisual = _promptVisual.RemoveAt(0);
            
            // After a small delay remove typed character
            yield return new WaitForSeconds(0.1f);
            Destroy(typedCharVisual.gameObject);
        }
    }
}