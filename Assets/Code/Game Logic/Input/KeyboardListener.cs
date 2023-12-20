using UnityEngine;

namespace KeyboardCats.Input
{
    public abstract class KeyboardListener : MonoBehaviour
    {
        public void Start()
        {
            Keyboard.Instance.keyPressed.AddListener(OnKeyPressed);
        }

        public void OnDestroy()
        {
            Keyboard.Instance.keyPressed?.RemoveListener(OnKeyPressed);
        }

        protected abstract void OnKeyPressed(string key);
    }
}