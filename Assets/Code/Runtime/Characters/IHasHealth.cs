namespace KeyboardDefense.Characters
{
    public interface IHasHealth
    {
        public void TakeDamage(float damage);
        public float GetCurrentHealth();
    }
}