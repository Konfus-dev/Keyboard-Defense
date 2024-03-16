using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Prompts
{
    public class PromptEffectPlayer : MonoBehaviour
    {
        [SerializeField] 
        private float volume = 1;
        [SerializeField]
        private AudioClip failSound;
        [SerializeField]
        private AudioClip typeSound;
        [SerializeField]
        private AudioClip completeSound;
        
        private Prompt _prompt;
        private ISoundService _soundService;

        private void Awake()
        {
            _soundService = ServiceProvider.Instance.Get<ISoundService>();
            _prompt = GetComponent<Prompt>();
        } 
        
        private void Start()
        {
            _prompt.promptCompleted.AddListener(PlayEffectOnPromptCompleted);
            _prompt.promptCharacterCorrectlyTyped.AddListener(PlayEffectOnPromptType);
            _prompt.promptCharacterIncorrectlyTyped.AddListener(PlayEffectOnPromptFail);
        }
        
        private void PlayEffectOnPromptCompleted()
        {
            _soundService.PlaySound(completeSound, 0.8f, 1.2f, volume, volume);
        }

        private void PlayEffectOnPromptType()
        {
            _soundService.PlaySound(typeSound, 0.8f, 1.2f, volume, volume);
        }
        
        private void PlayEffectOnPromptFail()
        {
            _soundService.PlaySound(failSound, 0.8f, 1.2f, volume, volume);
        }
    }
}
