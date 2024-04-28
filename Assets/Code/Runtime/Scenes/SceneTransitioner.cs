using System.Collections;
using KeyboardDefense.Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KeyboardDefense.Scenes
{
    public class SceneTransitioner : MonoBehaviour
    {
        public UnityEvent onTransitionComplete;
        
        // TODO: put this into scene transition info SO! Then move this into our scene manager class or maybe make it static?
        [SerializeField]
        private float transitionDuration = 1.0f;
        [SerializeField]
        private Image transitionImage;

        private ISceneManager _sceneManager;
        private void Awake()
        {
            _sceneManager = ServiceProvider.Instance.Get<ISceneManager>();
        }
        
        private void Start()
        {
            _sceneManager.QuitGame.AddListener(OnQuitGame);
            _sceneManager.ChangedCurrentScene.AddListener(OnChangedScenes);
            transitionImage.gameObject.SetActive(true);
            StartCoroutine(FadeInRoutine(transitionDuration));
        }

        private void OnDestroy()
        {
            _sceneManager.QuitGame.RemoveListener(OnQuitGame);
            _sceneManager.ChangedCurrentScene.RemoveListener(OnChangedScenes);
        }

        private void OnQuitGame()
        {
            TransitionToQuit();
        }

        private void OnChangedScenes()
        {
            TransitionTo(_sceneManager.CurrentScene.SceneName);
        }

        private void TransitionTo(string sceneName)
        {
            StartCoroutine(FadeOutRoutine(sceneName, transitionDuration));
        }
        
        private void TransitionToQuit()
        {
            StartCoroutine(FadeOutRoutine(transitionDuration));
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
