using UnityEngine;

namespace KeyboardDefense.Services
{
    public abstract class GameService : MonoBehaviour, IGameService
    {
        public virtual void Register() { }

        public virtual void Unregister() { }

        protected virtual void OnDestroy()
        {
            if (Application.isPlaying)
            {
                Debug.Log($"Unregistering the service {GetType().Name} after being destroyed.");
                Unregister();
            }
        }
    }
    
    public abstract class GameService<T> : GameService where T : class, IGameService
    {
        private void Awake()
        {
            Register();
        }

        public override void Register()
        {
            ServiceProvider.Register<T>(this);
        }
        
        public override void Unregister()
        {
            ServiceProvider.Unregister<T>();
        }
    }
}
