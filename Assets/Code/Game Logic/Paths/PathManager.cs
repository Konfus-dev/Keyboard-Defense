using Konfus.Utility.Design_Patterns;
using UnityEngine.Splines;

namespace KeyboardDefense.Paths
{
    public class PathManager : Singleton<PathManager>
    {
        public SplineContainer path;
    }
}