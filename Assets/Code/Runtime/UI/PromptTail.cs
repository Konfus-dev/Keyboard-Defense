using UnityEngine;

namespace KeyboardDefense.UI
{
    [ExecuteInEditMode]
    public class PromptTail : MonoBehaviour
    {
        private Camera _camera;
        private LineRenderer _lineRenderer;
        private GameObject _objectToFollowInWorld;

        public void SetObjectToFollow(GameObject objToFollow)
        {
            _objectToFollowInWorld = objToFollow;
        }

        private void Awake()
        {
            _camera = Camera.main;
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            var objToFollow = _objectToFollowInWorld;
            if (!objToFollow)
            {
                _lineRenderer.enabled = false;
                return;
            }
            
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, objToFollow.transform.position);
            _lineRenderer.SetPosition(1, transform.position - new Vector3(0 ,0, -1) * 0.5f);
        }

        private void OnDisable()
        {
            _lineRenderer.SetPosition(0,Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}