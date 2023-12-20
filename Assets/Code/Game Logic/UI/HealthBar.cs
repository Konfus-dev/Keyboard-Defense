using UnityEngine;
using UnityEngine.UI;

namespace KeyboardCats.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField]
        private Image healthBar;

        private void Start()
        {
            healthBar.transform.localScale = Vector3.one;
        }

        public void OnHealthChanged(float currHealth, float maxHealth)
        {
            if (currHealth < 0) return;
            var healthBarScale = healthBar.transform.localScale;
            healthBar.transform.localScale = new Vector3(currHealth/maxHealth, healthBarScale.y, healthBarScale.z);
        }
    }
}