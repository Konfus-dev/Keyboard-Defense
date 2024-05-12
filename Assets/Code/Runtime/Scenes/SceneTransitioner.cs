using KeyboardDefense.Services;
using UnityEngine.Events;

namespace KeyboardDefense.Scenes
{
    public class SceneTransitioner : GameService<ISceneTransitioner>, ISceneTransitioner
    {
        public UnityEvent OnTransitionOutComplete { get; } = new();
        public UnityEvent OnTransitionInComplete { get; } = new();
        
        private TransitionImage _transitionImage;


        public void PlayTransitionOutOfScene(float transitionDuration)
        {
            _transitionImage ??= FindObjectOfType<TransitionImage>();
            StartCoroutine(_transitionImage.PlayFadeOutRoutine(transitionDuration));
            OnTransitionOutComplete.Invoke();
        }
        
        public void PlayTransitionIntoScene(float transitionDuration)
        {
            _transitionImage ??= FindObjectOfType<TransitionImage>();
            StartCoroutine(_transitionImage.PlayFadeInRoutine(transitionDuration));
            OnTransitionInComplete.Invoke();
        }
    }
}
