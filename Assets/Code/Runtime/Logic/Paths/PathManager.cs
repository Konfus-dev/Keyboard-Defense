using Konfus.Utility.Design_Patterns;
using UnityEngine.Splines;

namespace KeyboardDefense.Logic.Paths
{
    public class PathManager : Singleton<PathManager>
    {
        public SplineContainer path;
    }
}