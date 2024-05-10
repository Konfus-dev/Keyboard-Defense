using KeyboardDefense.Services;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KeyboardDefense.Scenes
{
    public class SceneManager : SingletonGameService<ISceneManager>, ISceneManager
    {
        public UnityEvent QuitGame { get; } = new UnityEvent();
        public UnityEvent ChangedCurrentScene { get; } = new UnityEvent();
        public SceneInfo CurrentScene { get; private set; }
        
        private ISceneTransitioner _sceneTransitioner;
        private bool _quitting;
        private bool _initializing;

        public void LoadScene(SceneInfo scene)
        {
            if (scene.SceneType == SceneType.QuitGame)
            {
                Quit();
            }
            else
            {
                CurrentScene = scene;
                ChangedCurrentScene.Invoke();
                if (_initializing) _sceneTransitioner.PlayTransitionOutOfScene(0);
                else _sceneTransitioner.PlayTransitionOutOfScene(1.5f);
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

        private void Start()
        {
            _initializing = true;
            _sceneTransitioner = ServiceProvider.Instance.Get<ISceneTransitioner>();
            _sceneTransitioner.OnTransitionOutComplete.AddListener(OnTransitionOutComplete);
            CurrentScene = ServiceProvider.Instance.Get<ICurrentSceneProvider>()?.CurrentScene;
            if (CurrentScene)
            {
                ReloadCurrentScene();
                _initializing = false;
            }
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
                var activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                if (CurrentScene.SceneName != activeScene.name)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(CurrentScene.SceneName, LoadSceneMode.Single);
                }
                if (CurrentScene.SceneDependencies != null)
                {
                    foreach (var additionalScene in CurrentScene.SceneDependencies)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(additionalScene.SceneName,
                            LoadSceneMode.Additive);
                    }
                }
                _sceneTransitioner.PlayTransitionIntoScene(1.0f);
            }
        }
    }
}
