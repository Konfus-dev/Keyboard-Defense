using UnityEngine;

namespace KeyboardDefense.Services
{
    public class ServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            GameService[] gameServices = FindObjectsByType<GameService>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            foreach (var service in gameServices) service.Register();
        }
    }
}