using UnityEngine;

namespace KeyboardDefense.Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        // Awake function that finds and registers all GameService instances
        private void Awake()
        {
            GameService[] gameServices = FindObjectsByType<GameService>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            foreach (var service in gameServices) service.Register();
        }
    }
}