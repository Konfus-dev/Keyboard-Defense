using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(Image))]
    public class TextCursor : MonoBehaviour
    {
        [SerializeField]
        private float blinkSpeed;
        [SerializeField]
        private TMP_Text text;
        
        private int _indexOfLastTypedCharacter = -1;
        private int _targetAlpha = 0;
        private float _currentAlpha = 1f;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void OnPromptCharacterCorrectlyTyped()
        {
            _indexOfLastTypedCharacter++;
        }

        public void OnPromptCharacterIncorrectlyTyped()
        {
            _indexOfLastTypedCharacter = -1;
        }

        private void Update()
        {
            UpdateAlpha();
            UpdatePosition(_indexOfLastTypedCharacter);
        }

        private void UpdateAlpha()
        {
            if (_currentAlpha == 0) _targetAlpha = 1;
            else if (_currentAlpha == 1) _targetAlpha = 0;

            _currentAlpha = Mathf.MoveTowards(_currentAlpha, _targetAlpha, blinkSpeed * Time.deltaTime);
            
            Color objectColor = _image.color;
            objectColor.a = _currentAlpha;
            _image.color = objectColor;
        }

        private void UpdatePosition(int indexOfLastTypedCharacter) 
        {
            TMP_TextInfo textInfo = text.textInfo;
            Vector3 charPosInLocalSpace = Vector3.zero;
            
            if (indexOfLastTypedCharacter == -1)
            {
                // Reset the cursor position to the beginning
                TMP_CharacterInfo charInfo = textInfo.characterInfo[0];
                charPosInLocalSpace = charInfo.bottomLeft;
            }
            else
            {
                // Update the cursor position to the last typed character
                TMP_CharacterInfo charInfo = textInfo.characterInfo[indexOfLastTypedCharacter];
                if (charInfo.character == ' ' && indexOfLastTypedCharacter + 1 < textInfo.characterCount) 
                {
                    // char is a space, need to shift over one more and anchor to the left
                    charInfo = textInfo.characterInfo[indexOfLastTypedCharacter + 1];
                    charPosInLocalSpace = charInfo.bottomLeft;
                }
                else
                {
                    // Reg char anchor to the right
                    charPosInLocalSpace = charInfo.bottomRight;
                }
            }
            
            var wordLocationInWorld = text.transform.TransformPoint(charPosInLocalSpace);
            transform.position = new Vector3(wordLocationInWorld.x, transform.position.y, transform.position.z);
        }
    }
}