using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Input
{
    public abstract class KeyboardListener : MonoBehaviour
    {
        private IPlayer _player;
        private void Awake()
        {
            _player = ServiceProvider.Instance.Get<IPlayer>();
        }
        
        protected virtual void Start()
        {
            _player.KeyPressed.AddListener(OnKeyPressed);
        }

        protected virtual void OnDestroy()
        {
            _player.KeyPressed?.RemoveListener(OnKeyPressed);
        }

        protected abstract void OnKeyPressed(string key);
    }
}