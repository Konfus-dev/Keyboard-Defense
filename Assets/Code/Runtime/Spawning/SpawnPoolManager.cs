using System.Collections.Generic;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Spawning
{
    public class SpawnPoolManager : GameService<ISpawnService>, ISpawnService
    {
        [Tooltip("The pools of GameObjects to spawn from.")]
        public List<SpawnPool> pools;

        /// <summary>
        /// stores queues for pools by cref="MartianChild.Utility.Pool.tag".
        /// </summary>
        private Dictionary<string, Queue<GameObject>> _poolDict;

        private void Start()
        {
            CreatePools();
        }

        /// <summary>
        /// <para> Spawns object from pool at a specified gridPosition and rotation. </para>
        /// <param name="key"> <see cref="SpawnPool.key"/> given to item in <see cref="SpawnPool"/>. </param>
        /// <param name="position"> Position to spawn object. </param>
        /// <param name="rotation"> Rotation to spawn object. </param>
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            string key = prefab.name;
            if (!_poolDict.ContainsKey(key))
            {
                throw new KeyNotFoundException("No pool with key: " + key + " found!");
            }
            
            GameObject gameObj = _poolDict[key].Dequeue();
            gameObj.transform.position = position;
            gameObj.transform.rotation = rotation;
            gameObj.SetActive(true);
            _poolDict[key].Enqueue(gameObj);
            
            return gameObj;
        }

        private void CreatePools()
        {
            _poolDict = new Dictionary<string, Queue<GameObject>>();

            foreach (SpawnPool pool in pools)
            {
                Queue<GameObject> objPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab, pool.storage, true);
                    obj.name += i;
                    obj.SetActive(false);
                    objPool.Enqueue(obj);
                }

                _poolDict.Add(pool.prefab.name, objPool);
            }
        }
    }
}