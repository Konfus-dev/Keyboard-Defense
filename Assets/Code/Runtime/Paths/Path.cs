using KeyboardDefense.Services;
using UnityEngine;
using UnityEngine.Splines;

namespace KeyboardDefense.Paths
{
    public class Path : GameService<IPathProvider>, IPathProvider
    {
        [SerializeField]
        private SplineContainer spline;
        public SplineContainer Spline => spline;
    }
}