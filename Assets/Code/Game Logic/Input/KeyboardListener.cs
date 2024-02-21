using UnityEngine;

namespace KeyboardDefense.Input
{
    public abstract class KeyboardListener : MonoBehaviour
    {
        private void Start()
        {
            Player.Instance.keyPressed.AddListener(OnKeyPressed);
        }

        private void OnDestroy()
        {
            Player.Instance.keyPressed?.RemoveListener(OnKeyPressed);
        }

        protected abstract void OnKeyPressed(string key);
    }
}