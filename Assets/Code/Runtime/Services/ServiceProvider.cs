using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KeyboardDefense.Services
{
    // TODO: Doesn't need to be a singleton!!! Make this a static class!
    public static class ServiceProvider
    {
        private static Dictionary<Type, IGameService> _services = new Dictionary<Type, IGameService>();
        
        public static void Register<T>(IGameService service) where T : class, IGameService
        {
            _services ??= new Dictionary<Type, IGameService>();
            _services[typeof(T)] = service;
        }

        public static void Unregister<T>(T service) where T : class, IGameService
        {
            Unregister<T>();
        }
        
        public static void Unregister<T>() where T : class, IGameService
        {
            _services.Remove(typeof(T));
        }
        
        public static T Get<T>() where T : class, IGameService
        {
            if (!_services.TryGetValue(typeof(T), out IGameService gameService))
            {
                Debug.LogWarning($"No service registered for type {typeof(T).Name}");
                return null;
            }

            return (T)gameService;
        }
        
        public static IGameService[] GetAllRegisteredServices() => _services.Values.ToArray();
    }
}
