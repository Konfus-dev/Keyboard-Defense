using System;
using KeyboardDefense.Player.Input;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(MouseEventListener))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField, TextArea(minLines:2, maxLines:2)]
        private string header;
        [SerializeField, TextArea]
        private string content;

        private ITooltipService _tooltipService;
        private MouseEventListener _eventListener;

        public void Set(string tooltipContent, string tooltipHeader = "")
        {
            content = tooltipContent;
            header = tooltipHeader;
        }

        private void Awake()
        {
            _tooltipService = ServiceProvider.Instance.Get<ITooltipService>();
            _eventListener = GetComponent<MouseEventListener>();
        }
        
        private void Start()
        {
            _eventListener.mouseEnter.AddListener(OnStartHover);
            _eventListener.mouseExit.AddListener(OnStopHover);
        }
        
        private void OnStartHover()
        {
            _tooltipService.Show(content, header);
        }

        private void OnStopHover()
        {
            _tooltipService.Hide();
        }
    }
}