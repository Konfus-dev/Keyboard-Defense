using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardDefense.Spawning
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private SpawnData[] spawnData;
        [SerializeField]
        private SpawnPoolManager spawnService;

        private bool _isSpawning = true;
        
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
                    spawnService.Spawn(spawnable.Prefab, new Vector3(0, 0, -100), Quaternion.identity);
                    break;
                }
            }
        }
    }
}