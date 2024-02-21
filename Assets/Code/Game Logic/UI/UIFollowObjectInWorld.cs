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
        
        private void Start()
        {
            _camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
            _rt = GetComponent<RectTransform> ();
        }
     
        private void Update()
        {
            if (!objToFollow) return;
            _pos = RectTransformUtility.WorldToScreenPoint(_camera, objToFollow.transform.position);
            _rt.position = _pos + followOffset;
        }
    }
}
