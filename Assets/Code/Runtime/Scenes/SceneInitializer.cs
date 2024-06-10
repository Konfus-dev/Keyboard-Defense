using KeyboardDefense.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeyboardDefense.Scenes
{
    [RequireComponent(typeof(CurrentSceneProvider))]
    [RequireComponent(typeof(ServiceInitializer))]
    public class SceneInitializer : MonoBehaviour
    {
        const string PLAYER_SCENE_NAME = "Player";
        const string UNIVERSAL_UI_SCENE_NAME = "Universal UI";
        const string GAMEPLAY_UI_SCENE_NAME = "Gameplay UI";
        const string UNIVERSAL_SERVICES_SCENE_NAME = "Universal Services";
        const string GAMEPLAY_SERVICES_SCENE_NAME = "Gameplay Services";
        
        private ServiceInitializer _serviceInitializer;
        private CurrentSceneProvider _currentSceneInfoProvider;

        private void Awake()
        {
            // Register service from required scenes...
            _serviceInitializer = GetComponent<ServiceInitializer>();
            _currentSceneInfoProvider = GetComponent<CurrentSceneProvider>();
            
            // Load required scenes if not already opened...
            UnityEngine.SceneManagement.SceneManager.LoadScene(UNIVERSAL_SERVICES_SCENE_NAME, LoadSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.LoadScene(PLAYER_SCENE_NAME, LoadSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UNIVERSAL_UI_SCENE_NAME, LoadSceneMode.Additive);
            
            // If we are in a level, load gameplay scenes...
            if (_currentSceneInfoProvider.CurrentScene.SceneType == SceneType.Level)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(GAMEPLAY_UI_SCENE_NAME, LoadSceneMode.Additive);
                UnityEngine.SceneManagement.SceneManager.LoadScene(GAMEPLAY_SERVICES_SCENE_NAME, LoadSceneMode.Additive);
            }
        }
        
        private void Start()
        {
            _serviceInitializer.FindAndRegisterServices();
        }
    }
}