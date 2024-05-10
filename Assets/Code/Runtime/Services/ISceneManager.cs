using KeyboardDefense.Services;
using UnityEngine.Events;

namespace KeyboardDefense.Scenes
{
    public interface ISceneManager : IGameService
    {
        public UnityEvent QuitGame { get; }
        UnityEvent ChangedCurrentScene { get; }
        SceneInfo CurrentScene { get; }
        void LoadScene(SceneInfo sceneName);
        void ReloadCurrentScene();
        void Quit();
    }
}