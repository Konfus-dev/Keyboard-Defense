using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KeyboardDefense.Scenes
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public UnityEvent onTransitionComplete;
        
        [SerializeField]
        private float transitionDuration = 1.0f;
        [SerializeField]
        private Image transitionImage;

        public void TransitionTo(string sceneName)
        {
            StartCoroutine(FadeOutRoutine(sceneName, transitionDuration));
        }
        
        public void TransitionToQuit()
        {
            StartCoroutine(FadeOutRoutine(transitionDuration/2));
        }
        
        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLoadedScene;
        }
        
        private void OnLoadedScene(Scene scene, LoadSceneMode loadSceneMode)
        {
            StartCoroutine(FadeInRoutine(transitionDuration));
        }

        private IEnumerator FadeInRoutine(float fadeDuration)
        {
            yield return new WaitForSeconds(0.1f);
            onTransitionComplete.Invoke();
            yield return FadeRoutine(false, fadeDuration);
        }

        private IEnumerator FadeOutRoutine(float fadeDuration)
        {
            yield return FadeRoutine(true, fadeDuration);
            onTransitionComplete.Invoke();
            yield return new WaitForSeconds(0.1f);
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        private IEnumerator FadeOutRoutine(string sceneName, float fadeDuration)
        {
            yield return FadeRoutine(true, fadeDuration);
            yield return new WaitForSeconds(0.1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        
        private IEnumerator FadeRoutine(bool fadeOut, float fadeDuration)
        {
            Color startColor = transitionImage.color;
            Color targetColor = fadeOut 
                ? new Color(startColor.r, startColor.g, startColor.b, 1) 
                : new Color(startColor.r, startColor.g, startColor.b, 0);

            float startTime = Time.time;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                float t = elapsedTime / fadeDuration;
                transitionImage.color = Color.Lerp(startColor, targetColor, t);
                elapsedTime = Time.time - startTime;
                yield return null;
            }
            
            transitionImage.color = targetColor;
        }
    }

}
