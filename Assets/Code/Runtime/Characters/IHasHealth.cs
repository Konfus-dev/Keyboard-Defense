namespace KeyboardDefense.Characters
{
    public interface IHasHealth
    {
        public void TakeDamage(int damage);
        public int GetCurrentHealth();
    }
}