namespace KeyboardDefense.Logic.Spawning
{
    // TODO: make use of this!!!
    /// <summary>
    /// Interface for anything being spawned from a object pool
    /// </summary>
    public interface ISpawnable
    {
        void OnSpawn();
        void OnDespawn();
    }
}