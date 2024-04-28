using KeyboardDefense.Services;
using UnityEngine.Events;

namespace KeyboardDefense.Scenes
{
    public class SceneManager : SingletonGameService<ISceneManager>, ISceneManager
    {
        public UnityEvent QuitGame { get; } = new UnityEvent();
        public UnityEvent ChangedCurrentScene { get; } = new UnityEvent();
        
        public SceneInfo CurrentScene { get; private set; }
        
        public void LoadScene(SceneInfo scene)
        {
            CurrentScene = scene;
            ChangedCurrentScene.Invoke();
        }

        public void ReloadCurrentScene()
        {
            LoadScene(CurrentScene);
        }

        public void Quit()
        {
            QuitGame.Invoke();
        }
    }
}
