using KeyboardDefense.Characters;
using KeyboardDefense.Input;

namespace KeyboardDefense.Services
{
    public interface IPlayer : IGameService, IHasHealth
    {
        KeyPressedEvent KeyPressed { get; }
        HealthChangedEvent HealthChanged { get; }
    }
}