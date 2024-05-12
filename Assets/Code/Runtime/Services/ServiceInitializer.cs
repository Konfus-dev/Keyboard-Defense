﻿using System.Linq;
using UnityEngine;

namespace KeyboardDefense.Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        public void FindAndRegisterServices()
        {
            IGameService[] gameServices = FindObjectsByType<MonoBehaviour>(
                sortMode: FindObjectsSortMode.None, 
                findObjectsInactive: FindObjectsInactive.Include).OfType<IGameService>().ToArray();
            foreach (var service in gameServices) service.Register();
        }
    }
}