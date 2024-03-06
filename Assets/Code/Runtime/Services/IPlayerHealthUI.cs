namespace KeyboardDefense.Services
{
    public interface IPlayerHealthUI
    {
        void OnPlayerHealthChanged(float currHealth, float maxHealth);
    }
}