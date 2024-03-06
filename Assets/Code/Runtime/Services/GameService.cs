using System;
using UnityEngine;

namespace KeyboardDefense.Services
{
    public class GameService : MonoBehaviour
    {
        public virtual void Register() { }

        public virtual void Unregister() { }

        protected virtual void OnDestroy()
        {
            Unregister();
        }
    }
    
    public class GameService<T> : GameService
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
