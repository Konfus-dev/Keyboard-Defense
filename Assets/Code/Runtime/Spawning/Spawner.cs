using System;
using KeyboardDefense.Services;
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
                if (randomValue <= cumulativeChance)
                {
                    // Spawn the selected prefab
                    spawnService.Spawn(spawnable.Prefab, transform.position, Quaternion.identity);
                    break;
                }
            }
        }
    }
}