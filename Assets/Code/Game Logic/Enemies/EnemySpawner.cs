using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace KeyboardCats.Enemies
{
    // TODO: create an intelligent spawning system that spawns enemies in waves and at intervals based off difficulty!
    public class EnemySpawner : MonoBehaviour
    {
        public UnityEvent enemySpawned;
        
        [Header("Settings")]
        [SerializeField] 
        private float spawnInterval;
        [Space]
        [SerializeField]
        private SplineContainer spawnedEnemyPath;
        [SerializeField] 
        private Transform spawnPosition;
        [Space]
        [SerializeField]
        private GameObject[] spawnPool;

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
                SpawnRandomEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnRandomEnemy()
        {
            // Spawn random enemy
            var spawnIndex = Random.Range(0, spawnPool.Length);
            var enemy = Instantiate(spawnPool[spawnIndex], spawnPosition.position, spawnPosition.rotation);
            enemy.GetComponent<Enemy>().OnSpawn(spawnedEnemyPath);
            
            enemySpawned.Invoke();
        }
    }
}
