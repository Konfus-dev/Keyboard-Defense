using UnityEngine;

namespace KeyboardDefense.UI
{
    public class UIFollowObjectInWorld : MonoBehaviour
    {
        [SerializeField]
        private GameObject objToFollow;
        [SerializeField] 
        private Vector2 followOffset;
     
        private Camera _camera;
        private RectTransform _rt;
        private Vector2 _pos;
        
        public void SetObjectToFollow(GameObject o)
        {
            objToFollow = o;
        }

        public void SetFollowOffset(Vector2 offset)
        {
            followOffset = offset;
        }
        
        public GameObject GetObjectToFollow()
        {
            return objToFollow;
        }

        public Vector2 GetFollowOffset()
        {
            return followOffset;
        }
        
        private void Start()
        {
            _camera = Camera.main;
            _rt = GetComponent<RectTransform>();
        }
     
        private void Update()
        {
            if (!objToFollow) return;
            _pos = RectTransformUtility.WorldToScreenPoint(_camera, objToFollow.transform.position);
            _rt.position = _pos + followOffset;
        }
    }
}
