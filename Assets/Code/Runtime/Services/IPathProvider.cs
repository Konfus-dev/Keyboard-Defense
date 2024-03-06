using UnityEngine.Splines;

namespace KeyboardDefense.Services
{
    public interface IPathProvider
    {
        SplineContainer Spline { get; }
    }
}