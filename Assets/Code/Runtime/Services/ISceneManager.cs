using KeyboardDefense.Scenes;
using UnityEngine.Events;

namespace KeyboardDefense.Services
{
    public interface ISceneManager : IGameService
    {
        public UnityEvent QuitGame { get; }
        UnityEvent ChangedCurrentScene { get; }
        
        SceneInfo CurrentScene { get; }
        
        void Initialize(SceneInfo startingScene);
        void LoadScene(SceneInfo sceneName);
        void ReloadCurrentScene();
        void Quit();
    }
}