using UnityEngine;
using UnityEngine.Splines;

namespace KeyboardCats.Enemies
{
    public class SplineMovement : MonoBehaviour
    {
        [SerializeField]
        private SplineContainer splineContainer;

        private SplinePath<Spline> _splinePath;
        private float _elapsedTime;
        private float _speed;
        private bool _isMoving;
        private Vector3 _offset;

        public void SetContainer(SplineContainer container)
        {
            splineContainer = container;
            _splinePath = new SplinePath<Spline>(splineContainer.Splines);
        }

        public void SetOffset(Vector3 offset)
        {
            _offset = offset;
        }
        
        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void Move()
        {
            _isMoving = true;
        }
        
        public void Stop()
        {
            _isMoving = false;
        }
        
        private void Start()
        {
            if (splineContainer != null) 
                _splinePath = new SplinePath<Spline>(splineContainer.Splines);
        }
        
        private void Update()
        {
            UpdateTransform();
        }

        private void UpdateTransform()
        {
            if (splineContainer == null || _speed == 0 || !_isMoving) return;
            
            // Calculate pos along curve
            _elapsedTime += Time.fixedDeltaTime;
            var duration = splineContainer.Spline.GetLength() / _speed;
            var t = Mathf.Min(_elapsedTime, duration);
            t /= duration;
            
            // Get rotation
            var forward = Vector3.Normalize(splineContainer.EvaluateTangent(_splinePath, t));
            var up = splineContainer.EvaluateUpVector(_splinePath, t);
            var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(transform.forward, transform.up));
            var rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;
            
            // Get position
            var position = ((Vector3)splineContainer.EvaluatePosition(_splinePath, t)) + _offset;

            // Set transform pos and rot
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}