using KeyboardDefense.Characters;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.UI
{
    [RequireComponent(typeof(HealthBar))]
    public class PlayerHealthBar : MonoBehaviour
    {
        private HealthBar _healthBar;

        private void Awake()
        {
            _healthBar = GetComponent<HealthBar>();
        }

        private void Start()
        {
            var player = ServiceProvider.Get<IPlayer>();
            player.HealthChanged.AddListener(OnPlayerHealthChanged);
            OnPlayerHealthChanged(player.GetCurrentHealth(), ((Character)player).GetStats().Health);
        }

        private void OnPlayerHealthChanged(int currHealth, int maxHealth)
        {
            _healthBar.OnHealthChanged(currHealth, maxHealth);
        }
    }
}