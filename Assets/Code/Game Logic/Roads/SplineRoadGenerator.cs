using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace KeyboardCats.Roads
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SplineRoadGenerator : MonoBehaviour
    {
        [SerializeField]
        private SplineContainer spline;
        [SerializeField]
        private int numSegments = 20;
        [SerializeField]
        private float width = 2f;
        [SerializeField]
        private float height = 0.1f;

        private bool _isDirty;
        private MeshFilter _meshFilter;
        
        private void Awake()
        {
            _meshFilter = gameObject.GetComponent<MeshFilter>();
        }

        private void Start()
        {
            GenerateMesh();
        }
        
        private void GenerateMesh()
        {
            var mesh = new Mesh();
            Vector3 splineLastKnotWorldPos = spline.Spline.Knots.Last().Position;
            
            // Vertices and UVs
            var vertices = new Vector3[numSegments * 2];
            var uv = new Vector2[numSegments * 2];
            for (var i = 0; i < numSegments; i++)
            {
                var t = i / (float)(numSegments - 1);
                Vector3 splinePoint = spline.EvaluatePosition(t);
                Vector3 splineDirection = spline.EvaluateTangent(t);

                Vector3 offset = Vector3.Cross(Vector3.up, splineDirection).normalized * width;

                vertices[i * 2] = (splinePoint - offset * 0.5f + Vector3.up * (height * 0.5f)) + splineLastKnotWorldPos;
                vertices[i * 2 + 1] = (splinePoint + offset * 0.5f + Vector3.up * (height * 0.5f)) + splineLastKnotWorldPos;

                uv[i * 2] = new Vector2(t, 0f);
                uv[i * 2 + 1] = new Vector2(t, 1f);
            }

            // Triangles
            var triangles = new int[(numSegments - 1) * 6];
            for (int i = 0, ti = 0; i < numSegments - 1; i++, ti += 6)
            {
                triangles[ti] = i * 2;
                triangles[ti + 1] = (i + 1) * 2;
                triangles[ti + 2] = i * 2 + 1;

                triangles[ti + 3] = (i + 1) * 2;
                triangles[ti + 4] = (i + 1) * 2 + 1;
                triangles[ti + 5] = i * 2 + 1;
            }

            // Assigning data to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            
            // Set mesh
            _meshFilter.mesh = mesh;
        }

#if UNITY_EDITOR
        private void Update()
        {
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