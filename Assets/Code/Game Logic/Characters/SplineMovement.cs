using System;
using KeyboardDefense.Paths;
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
        private SplinePath<Spline> _splinePath;
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
        }

        private void OnEnable()
        {
            _elapsedTime = 0f;
            SetSpeed(_character.GetStats().MoveSpeed);
            SetPath(PathManager.Instance.path);
            GenerateRandomOffset();
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
            Vector3 splineDirection = _splinePath.EvaluateTangent(t);
            
            // Get position
            var position = ((Vector3)_splineContainer.EvaluatePosition(_splinePath, t)) + _offset;

            // Set transform pos and rot
            transform.position = position;
            transform.rotation = Quaternion.Euler(splineDirection);
        }
        
        private void SetPath(SplineContainer container)
        {
            _splineContainer = container;
            _splinePath = new SplinePath<Spline>(_splineContainer.Splines);
        }

        private void GenerateRandomOffset()
        {
            var randFollowPathOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
            _offset = randFollowPathOffset;
        }
    }
}