using UnityEngine;
using UnityEngine.Splines;

namespace KeyboardDefense.Paths
{
    [RequireComponent(typeof(LineRenderer))]
    public class SplineTrailGenerator : MonoBehaviour
    {
        [SerializeField]
        private int numberOfPoints = 50;

        private LineRenderer _lineRenderer;
        private SplineContainer _spline;
        
        private void Awake()
        {
            _spline = GetComponent<SplineContainer>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            Generate();
        }

        private void OnValidate()
        {
            _spline = GetComponent<SplineContainer>();
            _lineRenderer = GetComponent<LineRenderer>();
            Generate();
        }

        private void Generate()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = numberOfPoints;

            Vector3[] points = new Vector3[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                float t = (float)i / (numberOfPoints - 1);
                points[i] = _spline.EvaluatePosition(t);
            }

            _lineRenderer.SetPositions(points);
        }
    }
}