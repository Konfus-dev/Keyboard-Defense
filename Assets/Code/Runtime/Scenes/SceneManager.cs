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
        private bool _quitting = false;

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
                _sceneTransitioner.PlayTransitionOutOfScene(1.5f);
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
            _sceneTransitioner = ServiceProvider.Instance.Get<ISceneTransitioner>();
            _sceneTransitioner.OnTransitionOutComplete.AddListener(OnTransitionOutComplete);
            CurrentScene = ServiceProvider.Instance.Get<ICurrentSceneProvider>()?.CurrentScene;
            if (CurrentScene) ReloadCurrentScene();
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
                UnityEngine.SceneManagement.SceneManager.LoadScene(CurrentScene.SceneName, LoadSceneMode.Single);
                foreach (var additionalScene in CurrentScene.SceneDependencies)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(additionalScene.SceneName, LoadSceneMode.Additive);
                }
                _sceneTransitioner.PlayTransitionIntoScene(1.0f);
            }
        }
    }
}
