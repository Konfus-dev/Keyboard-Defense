using System;
using UnityEngine;

namespace KeyboardDefense.UI
{
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

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _lineRenderer.SetPosition(0, _uiFollowObjectInWorld.GetObjectToFollow().transform.position);
            _lineRenderer.SetPosition(1, _camera.ScreenToWorldPoint(new Vector3(transform.position.x, transform.position.y, 10)));
        }

        private void OnDisable()
        {
            _lineRenderer.SetPosition(0,Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}