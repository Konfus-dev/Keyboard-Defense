using UnityEngine;

namespace KeyboardDefense.UI
{
    public class PromptUI : MonoBehaviour
    {
        private void Start()
        {
            PromptUIPositionManager.Instance.RegisterNewPrompt(gameObject);
        }
    }
}