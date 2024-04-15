using UnityEngine;

namespace KeyboardDefense.UI
{
    [ExecuteInEditMode]
    public class PromptTail : MonoBehaviour
    {
        private Camera _camera;
        private LineRenderer _lineRenderer;
        private UIFollowObjectInWorld _uiFollowObjectInWorld;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _uiFollowObjectInWorld = GetComponentInParent<UIFollowObjectInWorld>();
        }

        private void OnEnable()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var objToFollow = _uiFollowObjectInWorld.GetObjectToFollow();
            if (!objToFollow)
            {
                _lineRenderer.enabled = false;
                return;
            }
            
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, objToFollow.transform.position);
            _lineRenderer.SetPosition(1, _camera.ScreenToWorldPoint(new Vector3(transform.position.x, transform.position.y, 10)));
        }

        private void OnDisable()
        {
            _lineRenderer.SetPosition(0,Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}