using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Input
{
    public abstract class KeyboardListener : MonoBehaviour
    {
        private IPlayer _player;

        protected virtual void Start()
        {
            _player = ServiceProvider.Instance.Get<IPlayer>();
            _player.KeyPressed.AddListener(OnKeyPressed);
        }

        protected virtual void OnDestroy()
        {
            _player?.KeyPressed?.RemoveListener(OnKeyPressed);
        }

        protected abstract void OnKeyPressed(string key);
    }
}