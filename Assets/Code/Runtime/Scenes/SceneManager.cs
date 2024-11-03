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
                UnityEngine.Application.Quit();
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
        }

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
