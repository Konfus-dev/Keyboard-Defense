using System.Linq;
using KeyboardDefense.Services;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KeyboardDefense.Scenes
{
    public class SceneManager : SingletonGameService<ISceneManager>, ISceneManager
    {
        public UnityEvent QuitGame { get; } = new UnityEvent();
        public UnityEvent ChangedScene { get; } = new UnityEvent();
        public SceneInfo CurrentScene { get; private set; }
        
        private ISceneTransitioner _sceneTransitioner;
        private bool _quitting;

        private void Awake()
        {
            _sceneTransitioner = GetComponent<ISceneTransitioner>();
        }

        private void Start()
        {
            // Set current scene
            var currentSceneProvider = ServiceProvider.Get<ICurrentSceneProvider>();
            CurrentScene = currentSceneProvider.CurrentScene;
            ChangedScene.Invoke();
            
            // Play transition into current scene
            _sceneTransitioner.OnTransitionOutComplete.AddListener(OnTransitionOutComplete);
            _sceneTransitioner.PlayTransitionOutOfScene(0);
            OnTransitionOutComplete();
        }

        public void LoadScene(SceneInfo scene)
        {
            if (scene.SceneType == SceneType.QuitGame)
            {
                Quit();
            }
            else
            {
                _sceneTransitioner.PlayTransitionOutOfScene(1.5f);
                if (CurrentScene == scene) // Reloading current scene
                {
                    UnityEngine.SceneManagement.SceneManager.UnloadScene(CurrentScene.SceneName);
                }
                else // Setting new scene
                {
                    CurrentScene = scene;
                    ChangedScene.Invoke();
                }
            }
        }

        public void ReloadCurrentScene()
        {
            LoadScene(CurrentScene);
        }

        public void Quit()
        {
            _quitting = true;
            QuitGame.Invoke();
            _sceneTransitioner.PlayTransitionOutOfScene(0.5f);
        }

        private void OnTransitionOutComplete()
        {
            if (_quitting)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
            else
            {
                LoadCurrentScene();
                _sceneTransitioner.PlayTransitionIntoScene(1.0f);
            }
        }

        private void LoadCurrentScene()
        {
            var openScenes = GetOpenScenes();
            if (openScenes.All(s => s.name != CurrentScene.SceneName))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(CurrentScene.SceneName, LoadSceneMode.Single);
            }
            //LoadSceneDependencies(CurrentScene, openScenes);
        }

        /*
        private void LoadSceneDependencies(SceneInfo sceneInfo, Scene[] openScenes)
        {
            // No dependencies, return!
            if (sceneInfo.SceneDependencies.IsNullOrEmpty()) return;
            
            // We do have dependencies! Load them recursively...
            foreach (var additionalScene in sceneInfo.SceneDependencies)
            {
                var canOpenScene = additionalScene.SceneType != SceneType.DependencyContainer &&
                                   openScenes.All(s => s.name != additionalScene.SceneName);
                if (canOpenScene)
                {
                    // Validate to see if we can load the scene...
                    if (additionalScene.SceneName.IsNullOrEmpty())
                    {
                        Debug.LogWarning($"Scene name is null or empty for {additionalScene.name} " +
                                         "and this is not marked as a dependency container, skipping loading this scene and its dependencies...");
                        break;
                    }
                    
                    // If we aren't just a container for additional dependencies, load the scene...
                    UnityEngine.SceneManagement.SceneManager.LoadScene(additionalScene.SceneName, LoadSceneMode.Additive);
                }
                
                // Load dependencies of this dependency...
                LoadSceneDependencies(additionalScene, openScenes);
            }
        }
        */

        public Scene[] GetOpenScenes()
        {
            int countLoaded = UnityEngine.SceneManagement.SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[countLoaded];
 
            for (int i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            }

            return loadedScenes;
        }
    }
}
