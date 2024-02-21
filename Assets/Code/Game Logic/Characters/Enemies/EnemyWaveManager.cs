using System.Collections;
using KeyboardDefense.Spawning;
using Konfus.Utility.Design_Patterns;
using UnityEngine;

namespace KeyboardDefense.Characters.Enemies
{
    public class EnemyWaveManager : Singleton<EnemyWaveManager>
    {
        [SerializeField]
        private float firstWaveSpawnDelay = 0f; // Time till we spawn our first wave
        [SerializeField]
        private float waveInterval = 10f; // Time between waves
        [SerializeField]
        private float waveIntervalDecreaseRate = 0.1f; // Rate at which the wave interval decreases
        [SerializeField]
        private float enemySpawnInterval = 1f; // Time between spawning enemies
        [SerializeField]
        private int enemiesPerWave = 5; // Number of enemies per wave
        [SerializeField]
        private int enemiesIncreasePerWave = 2; // Number of enemies added per wave
        [SerializeField]
        private float waveIntervalMinimum = 2f; // Minimum time between waves

        private float _nextWaveTime; // Time for next wave
        private int _currentWave = 1; // Current wave number

        private void Start()
        {
            _nextWaveTime = Time.time + firstWaveSpawnDelay;
        }

        private void Update()
        {
            if (Time.time >= _nextWaveTime)
            {
                StartWave();
                _nextWaveTime = Time.time + waveInterval;
                DecreaseWaveInterval();
                IncreaseEnemySpawnCount();
            }
        }

        private void StartWave()
        {
            Debug.Log("Wave " + _currentWave + " started!");

            // Spawn enemies
            StartCoroutine(SpawnEnemies());

            // Increase current wave number
            _currentWave++;
        }

        private IEnumerator SpawnEnemies()
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                Spawner.Instance.RandomSpawn();
                yield return new WaitForSeconds(enemySpawnInterval);
            }
        }

        private void IncreaseEnemySpawnCount()
        {
            enemiesPerWave += enemiesIncreasePerWave;
        }

        private void DecreaseWaveInterval()
        {
            // Decrease wave interval over time
            waveInterval -= waveIntervalDecreaseRate;

            // Ensure wave interval doesn't go below minimum value
            waveInterval = Mathf.Max(waveInterval, waveIntervalMinimum);
        }
    }
}
