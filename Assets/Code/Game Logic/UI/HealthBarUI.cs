using UnityEngine;
using UnityEngine.UI;

namespace KeyboardDefense.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField]
        private Image healthBar;

        private float _maxWidth;

        private void Start()
        {
            _maxWidth = healthBar.rectTransform.rect.width;
        }

        public void OnHealthChanged(float currHealth, float maxHealth)
        {
            if (currHealth < 0) return;
            healthBar.rectTransform.sizeDelta = new Vector2(currHealth/maxHealth * _maxWidth, healthBar.rectTransform.rect.height);
        }
    }
}