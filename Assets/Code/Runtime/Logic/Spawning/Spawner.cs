using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardDefense.Logic.Spawning
{
    public class Spawner : Singleton<Spawner>
    {
        [SerializeField]
        private SpawnData[] spawnData;

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
                    ObjectPoolManager.Instance.SpawnFromPool(
                        spawnable.Prefab.name, transform.position, Quaternion.identity);
                    //Instantiate(spawnable.EnemyPrefab, transform.position, Quaternion.identity);
                    break;
                }
            }
        }
    }
}