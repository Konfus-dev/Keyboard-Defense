using KeyboardDefense.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeyboardDefense.Scenes
{
    [RequireComponent(typeof(CurrentSceneProvider))]
    [RequireComponent(typeof(ServiceInitializer))]
    public class SceneInitializer : MonoBehaviour
    {
        const string REQUIRED_SERVICES_SCENE_NAME = "Required Services";
        const string PLAYER_SCENE_NAME = "Player";
        const string BASE_UI_SCENE_NAME = "Base UI";
        
        private ServiceInitializer _serviceInitializer;
        private CurrentSceneProvider _currentSceneProvider;

        private void Awake()
        {
            // Register service from required scenes...
            _serviceInitializer = GetComponent<ServiceInitializer>();
            _currentSceneProvider = GetComponent<CurrentSceneProvider>();
            
            // Load required scenes...
            UnityEngine.SceneManagement.SceneManager.LoadScene(REQUIRED_SERVICES_SCENE_NAME, LoadSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.LoadScene(PLAYER_SCENE_NAME, LoadSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.LoadScene(BASE_UI_SCENE_NAME, LoadSceneMode.Additive);
        }

        private void Start()
        {
            // Register service required scenes...
            _serviceInitializer.FindAndRegisterServices();
            
            // Load current scenes dependencies...
            ServiceProvider.Instance.Get<ISceneManager>().Initialize(_currentSceneProvider.CurrentScene);
            
            // Register service from newly loaded scenes from dependencies...
            _serviceInitializer.FindAndRegisterServices();

            // Log registered services...
            var services = ServiceProvider.Instance.GetAllRegisteredServices();
            foreach (var gameService in services)
            {
                Debug.Log($"Registered service: {gameService.GetType().Name}");
            }
        }
    }
}