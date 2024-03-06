using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Characters
{
    [RequireComponent(typeof(Character))]
    public class PlayEffectOnDeath : MonoBehaviour
    {
        [SerializeField]
        private GameObject deathFxPrefab;

        private Character _character;
        private ISpawnService _spawnService;

        private void Awake()
        {
            _spawnService = ServiceProvider.Instance.Get<ISpawnService>();
            _character = GetComponent<Character>();
        } 
        
        private void Start()
        {
            _character.onDie.AddListener(PlayEffectOnCharacterDeath);
        }
        
        private void PlayEffectOnCharacterDeath()
        {
            var prefab =_spawnService.Spawn(deathFxPrefab, transform.position + Vector3.up, Quaternion.identity);
            prefab.GetComponent<ParticleSystem>().Play();
        }
    }
}