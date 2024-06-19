using UnityEngine;
using UnityEngine.Splines;

namespace KeyboardDefense.Paths
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(SplineContainer))]
    public class SplinePathGenerator : MonoBehaviour
    {
        [SerializeField, Min(0.1f)]
        private float width = 2f;
        [SerializeField, Min(1)]
        private int resolution = 20;
        [SerializeField, Min(0)]
        private float curvature = 0.1f;

        private bool _isDirty;
        private MeshFilter _meshFilter;
        private SplineContainer _spline;
        
        private void Awake()
        {
            _spline = GetComponent<SplineContainer>();
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            GenerateMesh();
        }
        
        private void GenerateMesh()
        {
            // Set path to desired curvature (flatness)
            transform.localScale = new Vector3(
                x: transform.localScale.x, 
                y: curvature, 
                z: transform.localScale.z);
            
            // Set mesh
            var mesh = new Mesh();
            _meshFilter.mesh = mesh;
            
            // Extrude mesh along spline to create path
            SplineMesh.Extrude(_spline.Splines, mesh, width, 10, resolution, true, new Vector2(0, 1));
        }

        // TODO: put in editor script!
#if UNITY_EDITOR
        private void Update()
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                _isDirty = true;
            }
            
            if (_isDirty)
            {
                GenerateMesh();
                _isDirty = false;
            }
        }
        private void OnValidate()
        {
            // Cannot directly generate the mesh as it causes
            // a warning because we are changing the mesh filters mesh in OnValidate
            // which causes SendMessage to be called, which causes a warning to be generated.
            _spline = GetComponent<SplineContainer>();
            _meshFilter = GetComponent<MeshFilter>();
            _isDirty = true;
        }

        private void OnEnable()
        {
            Spline.Changed += OnSplineChanged;
        }
        
        private void OnDisable()
        {
            Spline.Changed -= OnSplineChanged;
        }

        private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3)
        {
            GenerateMesh();
        }
#endif
    }
}