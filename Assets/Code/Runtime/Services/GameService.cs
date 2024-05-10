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
                Unregister();
            }
        }
    }
    
    public abstract class GameService<T> : GameService where T : class, IGameService
    {
        public override void Register()
        {
            ServiceProvider.Instance.Register<T>(this);
        }
        
        public override void Unregister()
        {
            ServiceProvider.Instance.Unregister<T>();
        }
    }
}
