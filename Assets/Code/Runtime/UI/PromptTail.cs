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

            var posA = objToFollow.transform.position + Vector3.up;
            var posB = !_camera.orthographic
                ? _camera.ScreenToWorldPoint(new Vector3(transform.position.x, transform.position.y, _camera.nearClipPlane))
                : transform.position - Vector3.back;
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, posA);
            _lineRenderer.SetPosition(1, posB);
        }

        private void OnDisable()
        {
            _lineRenderer.SetPosition(0,Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}