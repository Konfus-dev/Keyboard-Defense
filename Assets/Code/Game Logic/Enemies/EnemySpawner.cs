using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KeyboardCats.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] 
        private Transform target;
        [SerializeField] 
        private Transform spawnPosition;
        [SerializeField]
        private GameObject[] spawnPool;
        [SerializeField] 
        private float spawnInterval;

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
            var spawnIndex = Random.Range(0, spawnPool.Length);
            var enemy = Instantiate(spawnPool[spawnIndex]);
            enemy.GetComponent<Enemy>().SetTarget(target);
            enemy.transform.position = spawnPosition.position;
            enemy.SetActive(true);
        }
    }
}
