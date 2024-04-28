using KeyboardDefense.Characters;
using KeyboardDefense.Services;

namespace KeyboardDefense.Player
{
    // TODO: make game over state a service, then do game over subscribers...
    public class PlayerCastle : Character
    {
        private IPlayerHealthUI _playerHealthUI;

        private void Awake()
        {
            _playerHealthUI = ServiceProvider.Instance.Get<IPlayerHealthUI>();
        }

        private void Start()
        {
            onHurt.AddListener(OnHurt);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            SetState(State.Idle);
        }

        private void OnHurt()
        {
            var currHealth = GetCurrentHealth();
            var maxHealth = GetStats().Health;
            _playerHealthUI.OnPlayerHealthChanged(currHealth, maxHealth);
        }
    }
}
