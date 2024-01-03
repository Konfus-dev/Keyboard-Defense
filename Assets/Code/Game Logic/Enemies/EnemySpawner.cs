using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardCats.Enemies
{
    // TODO: create an intelligent spawning system that spawns enemies in waves and at intervals based off difficulty!
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] 
        private Transform spawnPosition;
        [SerializeField]
        private GameObject[] spawnPool;
        [SerializeField] 
        private float spawnInterval;

        [Header("Debug")]
        [SerializeField] 
        private bool spawnEnemies = true;

        private void Start()
        {
            StartCoroutine(SpawnEnemiesAtConstantInterval());
        }

        private IEnumerator SpawnEnemiesAtConstantInterval()
        {
            while (spawnEnemies)
            {
                yield return new WaitForSeconds(spawnInterval);
                SpawnRandomEnemy();
            }
        }

        private void SpawnRandomEnemy()
        {
            // Spawn random enemy
            var spawnIndex = Random.Range(0, spawnPool.Length);
            var enemy = Instantiate(spawnPool[spawnIndex], spawnPosition.position, spawnPosition.rotation);
            
            // Configure spawned enemy
            var randFollowPathOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
            enemy.GetComponent<SplineMovement>().SetOffset(randFollowPathOffset);
        }
    }
}
