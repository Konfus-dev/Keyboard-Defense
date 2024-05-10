using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TransitionImage : MonoBehaviour
{
    private Image _transitionImage;
    
    public IEnumerator PlayFadeInRoutine(float fadeDuration)
    {
        yield return new WaitForSeconds(0.1f);
        yield return FadeRoutine(false, fadeDuration);
    }

    public IEnumerator PlayFadeOutRoutine(float fadeDuration)
    {
        yield return FadeRoutine(true, fadeDuration);
    }
        
    private IEnumerator FadeRoutine(bool fadeOut, float fadeDuration)
    {
        Color startColor = _transitionImage.color;
        Color targetColor = fadeOut 
            ? new Color(startColor.r, startColor.g, startColor.b, 1) 
            : new Color(startColor.r, startColor.g, startColor.b, 0);

        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            _transitionImage.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime = Time.time - startTime;
            yield return null;
        }
            
        _transitionImage.color = targetColor;
    }

    private void Awake()
    {
        _transitionImage = GetComponent<Image>();
    }
}
