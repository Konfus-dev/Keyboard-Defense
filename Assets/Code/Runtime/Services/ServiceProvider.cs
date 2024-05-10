using System;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardDefense.Services
{
    // TODO: Doesn't need to be a singleton!!! Make this a static class!
    public class ServiceProvider
    {
        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ServiceProvider Instance { get; } = new ServiceProvider();
        
        private Dictionary<Type, object> _services;
        
        public void Register<T>(object service) where T : class, IGameService
        {
            _services ??= new Dictionary<Type, object>();
            _services[typeof(T)] = service;
        }

        public void Unregister<T>() where T : class, IGameService
        {
            _services.Remove(typeof(T));
        }
        
        public T Get<T>() where T : class, IGameService
        {
            if (!_services.TryGetValue(typeof(T), out object gameService))
            {
                Debug.LogWarning($"No service registered for type {typeof(T).Name}");
                return null;
            }

            return (T)gameService;
        }
    }
}
