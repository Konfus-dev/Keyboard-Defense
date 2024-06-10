using KeyboardDefense.Characters;
using KeyboardDefense.Services;
using UnityEngine;

namespace KeyboardDefense.Castles
{
    public class PlayerCastle : MonoBehaviour, IHasHealth
    {
        private IPlayer _player;

        public void TakeDamage(int damage)
        {
            _player.TakeDamage(damage);
        }

        public int GetCurrentHealth()
        {
            return _player.GetCurrentHealth();
        }

        private void Start()
        {
            _player = ServiceProvider.Get<IPlayer>();
        }
    }
}
