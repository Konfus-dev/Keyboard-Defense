using System.Collections;
using KeyboardDefense.Spawning;
using Konfus.Utility.Design_Patterns;
using UnityEngine;
using UnityEngine.Serialization;

namespace KeyboardDefense.Characters.Enemies
{
    // TODO: make this more intelligent!
    // i.e. keeps track of spawned enemies, when they are all dead there will be a countdown till next wave...
    // and have the settings are configured based on difficulty, stuff like that
    public class EnemyWaveManager : Singleton<EnemyWaveManager>
    {
        [SerializeField, Tooltip("Time till we spawn our first wave")]
        private float firstWaveSpawnDelay = 0f;
        [SerializeField, Tooltip("Starting time between waves in seconds")]
        private float startingTimeBetweenWaves = 10f;
        [SerializeField, Tooltip("Decrease of wave interval in seconds")]
        private float timeBetweenWavesDecreaseRatePerWave = 0.1f;
        [SerializeField, Tooltip("Minimum time between waves")]
        private float waveIntervalMinimum = 2f; 
        [SerializeField, Tooltip("Time between spawning enemies in seconds")]
        private float enemySpawnInterval = 1f;
        [SerializeField, Tooltip("Starting number of enemies per wave")]
        private int startingWaveEnemyCount = 5;
        [SerializeField, Tooltip("Number of enemies added per wave")]
        private int increaseInEnemiesPerWave = 2;

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
                _nextWaveTime = Time.time + startingTimeBetweenWaves;
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
            for (int i = 0; i < startingWaveEnemyCount; i++)
            {
                Spawner.Instance.RandomSpawn();
                yield return new WaitForSeconds(enemySpawnInterval);
            }
        }

        private void IncreaseEnemySpawnCount()
        {
            startingWaveEnemyCount += increaseInEnemiesPerWave;
        }

        private void DecreaseWaveInterval()
        {
            // Decrease wave interval over time
            startingTimeBetweenWaves -= timeBetweenWavesDecreaseRatePerWave;

            // Ensure wave interval doesn't go below minimum value
            startingTimeBetweenWaves = Mathf.Max(startingTimeBetweenWaves, waveIntervalMinimum);
        }
    }
}
