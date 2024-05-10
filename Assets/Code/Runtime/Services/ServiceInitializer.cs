using System.Linq;
using UnityEngine;

namespace KeyboardDefense.Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        // Awake function that finds and registers all GameService instances
        private void Awake()
        {
            IGameService[] gameServices = FindObjectsByType<MonoBehaviour>(
                sortMode: FindObjectsSortMode.None, 
                findObjectsInactive: FindObjectsInactive.Include).OfType<IGameService>().ToArray();
            foreach (var service in gameServices) service.Register();
        }
    }
}