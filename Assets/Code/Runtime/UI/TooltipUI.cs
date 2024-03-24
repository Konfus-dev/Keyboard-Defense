using KeyboardDefense.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardDefense.UI
{
    [ExecuteInEditMode, RequireComponent(typeof(LayoutElement), typeof(RectTransform))]
    public class TooltipUI : GameService<ITooltipService>, ITooltipService
    {
        [Header("Settings")]
        [SerializeField]
        private int characterWrapLimit;
    
        [Header("Dependencies")]
        [SerializeField]
        private TMP_Text header;
        [SerializeField]
        private TMP_Text content;
        
        private LayoutElement _layoutElement;
        private RectTransform _rectTransform;
        
        public void Show(string contentTxt, string headerTxt = "")
        {
            header.text = headerTxt;
            content.text = contentTxt;
            
            ScaleToFitText();
            MoveToMousePosition();
            
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _layoutElement = GetComponent<LayoutElement>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                Hide();
            }
        }

        private void Update()
        {
            if (Application.isPlaying) MoveToMousePosition();
            ScaleToFitText();
        }

        private void ScaleToFitText()
        {
            int headerLen = header.text.Length;
            int contentLen = content.text.Length;

            var layoutElementEnabled = (headerLen > characterWrapLimit) || (contentLen > characterWrapLimit);
            _layoutElement.enabled = layoutElementEnabled;
        }

        private void MoveToMousePosition()
        {
            Vector2 position = ((Vector2)Input.mousePosition) + new Vector2(0, _rectTransform.rect.height);

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            _rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }
    }
}
