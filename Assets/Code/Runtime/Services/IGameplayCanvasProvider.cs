using UnityEngine;

namespace KeyboardDefense.Services
{
    public interface IGameplayCanvasProvider : IGameService
    {
        Canvas GameplayCanvas { get; }
    }
}