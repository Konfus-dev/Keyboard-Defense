using KeyboardDefense.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardDefense.Spawning
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private SpawnData[] spawnData;
        
        //private ISpawnService _spawnService;
        private bool _isSpawning = true;

        private void Start()
        {
            //_spawnService = ServiceProvider.Get<ISpawnService>();
        }
        
        public void StopSpawning()
        {
            _isSpawning = false;
        }
        
        public void RandomSpawn()
        {
            // Calculate total spawn chance
            float totalSpawnChance = 0f;
            foreach (SpawnData spawnable in spawnData)
            {
                totalSpawnChance += spawnable.SpawnChance;
            }

            // Generate a random number between 0 and totalSpawnChance
            float randomValue = Random.Range(0f, totalSpawnChance);

            // Find the spawnable to spawn based on randomValue
            float cumulativeChance = 0f;
            foreach (SpawnData spawnable in spawnData)
            {
                cumulativeChance += spawnable.SpawnChance;
                if (randomValue <= cumulativeChance && _isSpawning)
                {
                    // Spawn the selected prefab under map... the enemy will move itself to the path...
                    // TODO: Bug with spawn/object pool service.
                    // The enemies don't seem to work right after re-use. For now just going to spawn them as I need them.
                    Instantiate(spawnable.Prefab, new Vector3(0, 0, -100), Quaternion.identity);
                    break;
                }
            }
        }
    }
}