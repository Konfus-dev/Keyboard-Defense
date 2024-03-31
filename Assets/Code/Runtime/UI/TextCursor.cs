using System;
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

        private void Start()
        {
            UpdatePosition(_indexOfLastTypedCharacter);
        }

        public void OnPromptCharacterCorrectlyTyped()
        {
            _indexOfLastTypedCharacter++;
            UpdatePosition(_indexOfLastTypedCharacter);
        }

        public void OnPromptCharacterIncorrectlyTyped()
        {
            _indexOfLastTypedCharacter = -1;
            UpdatePosition(_indexOfLastTypedCharacter);
        }

        private void Update()
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
                TMP_CharacterInfo charInfo = textInfo.characterInfo[0];
                charPosInLocalSpace = charInfo.bottomLeft;
            }
            else
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[indexOfLastTypedCharacter];
                charPosInLocalSpace = charInfo.bottomRight;
            }
            
            var wordLocationInWorld = text.transform.TransformPoint(charPosInLocalSpace);
            transform.position = new Vector3(wordLocationInWorld.x, transform.position.y, transform.position.z);
        }
    }
}