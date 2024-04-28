using UnityEngine.Events;

namespace KeyboardDefense.Scenes
{
    public interface ISceneManager
    {
        public UnityEvent QuitGame { get; }
        UnityEvent ChangedCurrentScene { get; }
        SceneInfo CurrentScene { get; }
        void LoadScene(SceneInfo sceneName);
        void ReloadCurrentScene();
        void Quit();
    }
}