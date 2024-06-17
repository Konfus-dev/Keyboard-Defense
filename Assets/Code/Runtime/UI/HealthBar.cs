using UnityEngine;
using UnityEngine.UI;

namespace KeyboardDefense.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image healthBar;
        [SerializeField]
        private float fullWidth;

        public void OnHealthChanged(float currHealth, float maxHealth)
        {
            if (currHealth < 0) return;
            healthBar.rectTransform.sizeDelta = new Vector2(currHealth/maxHealth * fullWidth, healthBar.rectTransform.rect.height);
        }
    }
}