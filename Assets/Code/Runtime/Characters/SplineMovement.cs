using KeyboardDefense.Services;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace KeyboardDefense.Characters
{
    [RequireComponent(typeof(Character))]
    public class SplineMovement : MonoBehaviour
    {
        private Character _character;
        private SplineContainer _splineContainer;
        private float _elapsedTime;
        private float _speed;
        private bool _isMoving;
        private Vector3 _offset;

        public float GetCurrentSpeed() => _speed;
        
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

        private void Awake()
        {
            _character = GetComponent<Character>();
            _splineContainer = ServiceProvider.Instance.Get<IPathProvider>().Spline;
        }

        private void Start()
        {
            SetSpeed(_character.GetStats().MoveSpeed);
            GenerateRandomOffset();
        }

        private void OnEnable()
        {
            _elapsedTime = 0f;
        }

        private void FixedUpdate()
        {
            UpdateTransform();
        }

        private void UpdateTransform()
        {
            if (_splineContainer == null || _speed == 0 || !_isMoving) return;
            
            // Calculate pos along curve
            _elapsedTime += Time.fixedDeltaTime;
            var duration = _splineContainer.Spline.GetLength() / _speed;
            var t = Mathf.Min(_elapsedTime, duration);
            t /= duration;
            
            // Get rotation
            Vector3 splineDirection = _splineContainer.EvaluateTangent(t);
            var rotation = Quaternion.LookRotation(splineDirection, transform.up);
            
            // Get position
            var position = (Vector3)_splineContainer.EvaluatePosition(t) + _offset;

            // Set transform pos and rot
            transform.position = position;
            transform.rotation = rotation;
        }

        private void GenerateRandomOffset()
        {
            var randFollowPathOffset = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-0.5f, 0.5f));
            _offset = randFollowPathOffset;
        }
    }
}