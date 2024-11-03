using KeyboardDefense.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TooltipManager : GameService<ITooltipService>, ITooltipService
    {
        [Header("Settings")]
        [SerializeField]
        private int characterWrapLimit;
        [SerializeField]
        private Vector2 offset;
    
        [Header("Dependencies")]
        [SerializeField]
        private TMP_Text header;
        [SerializeField]
        private TMP_Text content;
        [SerializeField]
        private GameObject visuals;
        
        private LayoutElement _layout;
        private RectTransform _rectTransform;
        
        public void Show(string contentTxt, string headerTxt = "")
        {
            if (visuals == null) return;
            
            header.text = headerTxt;
            content.text = contentTxt;
            
            ScaleToFitText();
            MoveToMousePosition();
            
            visuals.SetActive(true);
        }
        
        public void Hide()
        {
            if (visuals == null) return;
            visuals.SetActive(false);
        }

        private void Awake()
        {
            _layout = visuals.GetComponent<LayoutElement>();
            _rectTransform = visuals.GetComponent<RectTransform>();
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
            if (!_layout || !header || !content)
            {
                return;
            }
            
            if (Application.isPlaying) MoveToMousePosition();
            ScaleToFitText();
        }

        private void ScaleToFitText()
        {
            int headerLen = header.text.Length;
            int contentLen = content.text.Length;

            var layoutElementEnabled = (headerLen > characterWrapLimit) || (contentLen > characterWrapLimit);
            _layout.enabled = layoutElementEnabled;
        }

        private void MoveToMousePosition()
        {
            var rect = new Vector2(-_rectTransform.rect.width, -_rectTransform.rect.height);
            Vector2 position = ((Vector2)UnityEngine.Input.mousePosition) + rect + offset;

            position.x = Mathf.Clamp(position.x, 0, Screen.width - rect.x);
            position.y = Mathf.Clamp(position.y, 0, Screen.height - rect.y);

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            _rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }
    }
}
