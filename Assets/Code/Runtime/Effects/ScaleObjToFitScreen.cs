using UnityEngine;

namespace KeyboardDefense.UI
{
    public class ScaleObjectToFitScreen : MonoBehaviour
    {
        [SerializeField]
        private Renderer target;

        private void Update()
        {
            if (!target) return;
            
            // Get the screen size
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            // Calculate the scaling factor
            Bounds bounds = target.bounds;
            float objectWidth = bounds.size.x;
            float objectHeight = bounds.size.y;
            float scale = Mathf.Min(screenWidth / objectWidth, screenHeight / objectHeight);

            // Scale the object
            gameObject.transform.localScale *= scale;
        }
    }
}