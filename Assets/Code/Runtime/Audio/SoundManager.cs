using System.Collections;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Audio
{
    public class SoundManager : SingletonGameService<ISoundService>, ISoundService
    {
        [SerializeField]
        private AudioSource fxAudioSource;
        [SerializeField]
        private AudioSource ambianceAudioSource;
        [SerializeField]
        private AudioSource musicAudioSource;
    
        public void PlaySoundAtPoint(AudioClip clip, Vector3 position)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    
        public void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    
        public void PlaySound(AudioClip clip, float minPitch, float maxPitch, float minVolume, float maxVolume)
        {
            fxAudioSource.pitch = Random.Range(minPitch, maxPitch);
            fxAudioSource.volume = Random.Range(minVolume, maxVolume);
        
            fxAudioSource.PlayOneShot(clip);
        }
    
        public void ChangeMusic(AudioClip newClip, float fadeDuration)
        {
            StartCoroutine(FadeOutAndChange(musicAudioSource, newClip, fadeDuration));
        }
    
        public void ChangeAmbiance(AudioClip newClip, float fadeDuration)
        {
            StartCoroutine(FadeOutAndChange(ambianceAudioSource, newClip, fadeDuration));
        }
    
        private IEnumerator FadeOutAndChange(AudioSource audioSource, AudioClip newClip, float fadeDuration)
        {
            float startVolume = audioSource.volume;
            float timer = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }

            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();

            while (audioSource.volume < startVolume)
            {
                audioSource.volume += Time.deltaTime / fadeDuration;
                yield return null;
            }
        }
    }
}
