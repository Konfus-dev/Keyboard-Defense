using KeyboardDefense.Services;
using UnityEngine.Events;

namespace KeyboardDefense.Scenes
{
    public class SceneTransitioner : GameService<ISceneTransitioner>, ISceneTransitioner
    {
        public UnityEvent OnTransitionOutComplete { get; } = new();
        public UnityEvent OnTransitionInComplete { get; } = new();
        
        private TransitionImage _transitionImage;

        private void Awake()
        {
            _transitionImage =  FindObjectOfType<TransitionImage>();
        }

        public void PlayTransitionOutOfScene(float transitionDuration)
        {
            StartCoroutine(_transitionImage.PlayFadeOutRoutine(transitionDuration));
            OnTransitionOutComplete.Invoke();
        }
        
        public void PlayTransitionIntoScene(float transitionDuration)
        {
            StartCoroutine(_transitionImage.PlayFadeInRoutine(transitionDuration));
            OnTransitionInComplete.Invoke();
        }
    }
}
