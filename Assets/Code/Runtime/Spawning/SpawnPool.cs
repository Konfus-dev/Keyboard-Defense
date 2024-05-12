using UnityEngine;

namespace KeyboardDefense.Spawning
{
    [System.Serializable]
    public class SpawnPool
    {
        /// <summary>
        /// size of pool.
        /// </summary>
        public int size;
        /// <summary>
        /// prefab to instantiate objects from for a pool, the name also serves as the key for the pool.
        /// </summary>
        public GameObject prefab;
        /// <summary>
        /// place to parent and store objects in a pool.
        /// </summary>
        public Transform storage;
    }
}