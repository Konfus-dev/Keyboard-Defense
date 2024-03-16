using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardDefense.Services
{
    public interface IGameService
    {
        void Register();
        void Unregister();
    }
    
    public class GameService : MonoBehaviour, IGameService
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
    
    public abstract class GameService<T> : GameService
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
    
    public abstract class SingletonGameService<T> : GameService where T : class
    {
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance { get; private set; }
        
        public override void Register()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
                ServiceProvider.Instance.Register<T>(this);
            }
            else if (!ReferenceEquals(Instance, this))
            {
                Destroy(gameObject);
            }
        }
        
        public override void Unregister()
        {
            Instance = null;
            Destroy(gameObject);
            ServiceProvider.Instance.Unregister<T>();
        }
    }
}
