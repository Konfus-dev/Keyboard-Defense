using UnityEngine.Events;

namespace KeyboardDefense.Services
{
    public interface ISceneTransitioner : IGameService
    {
        UnityEvent OnTransitionOutComplete { get; }
        UnityEvent OnTransitionInComplete { get; }
        void PlayTransitionOutOfScene(float transitionDuration);
        void PlayTransitionIntoScene(float transitionDuration);
    }
}