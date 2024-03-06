using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(HealthBarUI))]
    public class PlayerHealthUI : GameService<IPlayerHealthUI>, IPlayerHealthUI
    {
        private HealthBarUI _healthBar;

        private void Awake()
        {
            _healthBar = GetComponent<HealthBarUI>();
        }
        
        public void OnPlayerHealthChanged(float currHealth, float maxHealth)
        {
            _healthBar.OnHealthChanged(currHealth, maxHealth);
        }
    }
}