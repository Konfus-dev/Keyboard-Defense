using UnityEngine;

namespace KeyboardDefense.Services
{
    public interface ISpawnService : IGameService
    {
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);
    }
}