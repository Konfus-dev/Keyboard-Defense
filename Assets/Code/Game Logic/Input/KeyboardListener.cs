using UnityEngine;

namespace KeyboardCats.Input
{
    public abstract class KeyboardListener : MonoBehaviour
    {
        public void Start()
        {
            Player.Instance.keyPressed.AddListener(OnKeyPressed);
        }

        public void OnDestroy()
        {
            Player.Instance.keyPressed?.RemoveListener(OnKeyPressed);
        }

        protected abstract void OnKeyPressed(string key);
    }
}