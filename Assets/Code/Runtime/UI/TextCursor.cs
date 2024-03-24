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
        
        private int _targetAlpha = 0;
        private float _currentAlpha = 1f;
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
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
    }
}