using UnityEngine;

namespace KeyboardDefense.Services
{
    public interface ISoundService : IGameService
    {
        void PlaySoundAtPoint(AudioClip clip, Vector3 position);
        void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume);
        void PlaySound(AudioClip clip, float minPitch, float maxPitch, float minVolume, float maxVolume);
        void ChangeMusic(AudioClip newClip, float fadeDuration);
        void ChangeAmbiance(AudioClip newClip, float fadeDuration);
    }
}