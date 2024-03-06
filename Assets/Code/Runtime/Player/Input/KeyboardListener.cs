using UnityEngine;

namespace KeyboardDefense.Player.Input
{
    public abstract class KeyboardListener : MonoBehaviour
    {
        protected virtual void Start()
        {
            Player.Instance.keyPressed.AddListener(OnKeyPressed);
        }

        protected virtual void OnDestroy()
        {
            Player.Instance.keyPressed?.RemoveListener(OnKeyPressed);
        }

        protected abstract void OnKeyPressed(string key);
    }
}