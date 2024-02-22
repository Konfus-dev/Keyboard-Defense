using KeyboardDefense.UI;

namespace KeyboardDefense.Characters.Castles
{
    public class PlayerCastle : Character
    {
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
            UIManager.Instance.PlayerHealthUI.OnHealthChanged(currHealth, maxHealth);
        }
    }
}
