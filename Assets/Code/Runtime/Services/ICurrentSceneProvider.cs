using KeyboardDefense.Scenes;

namespace KeyboardDefense.Services
{
    public interface ICurrentSceneProvider : IGameService
    {
        SceneInfo CurrentScene { get; }
    }
}