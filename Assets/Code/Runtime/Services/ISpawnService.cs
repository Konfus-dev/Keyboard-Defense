using UnityEngine;

namespace KeyboardDefense.Services
{
    public interface ISpawnService
    {
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);
    }
}