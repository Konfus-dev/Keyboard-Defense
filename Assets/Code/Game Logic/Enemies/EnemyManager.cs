using Konfus.Utility.Design_Patterns;
using UnityEngine;
using UnityEngine.Splines;

namespace KeyboardCats.Enemies
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        [SerializeField]
        private SplineContainer path;
        
        public SplineContainer GetPath()
        {
            return path;
        }
    }
}
