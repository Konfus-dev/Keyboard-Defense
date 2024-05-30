using System.Collections;
using System.Linq;
using KeyboardDefense.Services;
using KeyboardDefense.UI;
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
            StartCoroutine(PlayTransitionOutOfSceneRoutine(transitionDuration));
        }

        private IEnumerator PlayTransitionOutOfSceneRoutine(float transitionDuration)
        {
            yield return _transitionImage.PlayFadeOutRoutine(transitionDuration);
            OnTransitionOutComplete.Invoke();
        }
        
        public void PlayTransitionIntoScene(float transitionDuration)
        {
            StartCoroutine(PlayTransitionIntoSceneRoutine(transitionDuration));
        }
        
        private IEnumerator PlayTransitionIntoSceneRoutine(float transitionDuration)
        {
            yield return _transitionImage.PlayFadeInRoutine(transitionDuration);
            OnTransitionInComplete.Invoke();
        }
        
        private void Awake()
        {
            _transitionImage = GetComponentInChildren<TransitionImage>();
        }
    }
}
