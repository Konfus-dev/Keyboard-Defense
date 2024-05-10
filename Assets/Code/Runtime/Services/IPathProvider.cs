using UnityEngine.Splines;

namespace KeyboardDefense.Services
{
    public interface IPathProvider : IGameService
    {
        SplineContainer Spline { get; }
    }
}