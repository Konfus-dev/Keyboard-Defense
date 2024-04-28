using UnityEngine;

namespace KeyboardDefense.Effects
{
    public class RotateObject : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed = 50f;

        private void Update()
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}