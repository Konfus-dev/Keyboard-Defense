using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Characters
{
    [RequireComponent(typeof(Character))]
    public class PlayEffectOnDeath : MonoBehaviour
    {
        [SerializeField] 
        private float volume = 1;
        [SerializeField]
        private AudioClip sound;
        [SerializeField]
        private GameObject particleEffect;

        private Character _character;
        private ISpawnService _spawnService;
        private ISoundService _soundService;

        private void Awake()
        {
            _soundService = ServiceProvider.Instance.Get<ISoundService>();
            _spawnService = ServiceProvider.Instance.Get<ISpawnService>();
            _character = GetComponent<Character>();
        } 
        
        private void Start()
        {
            _character.onDie.AddListener(PlayEffectOnCharacterDeath);
        }
        
        private void PlayEffectOnCharacterDeath()
        {
            var position = transform.position;
            _soundService.PlaySoundAtPoint(sound, position, volume);
            var prefab =_spawnService.Spawn(particleEffect, position + Vector3.up, Quaternion.identity);
            prefab.GetComponent<ParticleSystem>().Play();
        }
    }
}