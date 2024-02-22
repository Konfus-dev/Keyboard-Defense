using UnityEngine;

namespace KeyboardDefense.Logic.Spawning
{
    [CreateAssetMenu(fileName = "New Spawn Data", menuName = "Keyboard Defense/New Spawn Data")]
    public class SpawnData : ScriptableObject
    {
        [SerializeField, Range(0, 1)]
        private float spawnChance;
        [SerializeField]
        private GameObject prefab;

        public float SpawnChance => spawnChance;
        public GameObject Prefab => prefab;
    }
}