using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Scenes
{
    public class CurrentSceneProvider : GameService<ICurrentSceneProvider>, ICurrentSceneProvider
    {
        [SerializeField]
        private SceneInfo currentScene;
        public SceneInfo CurrentScene => currentScene;
    }
}