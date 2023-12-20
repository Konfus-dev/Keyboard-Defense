using UnityEngine;

namespace KeyboardCats.UI
{
    [ExecuteInEditMode]
    public class Billboard : MonoBehaviour
    {
        private Transform _cameraToLookAt;

        private void Awake()
        {
            _cameraToLookAt = Camera.main.transform;
        }

        public void Update()
        {
            transform.LookAt(_cameraToLookAt);
        }
    }
}
