using UnityEngine;

namespace Assets.Scripts.Fov
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float fov;
        [SerializeField] float viewDistance;
        Mesh mesh;
        Vector3 origin;

        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void LateUpdate()
        {
            const int rayCount = 50;
            var angle = 0f;
            var angleIncrease = fov / rayCount;

            var vertices = new Vector3[rayCount + 1 + 1];
            var uv = new Vector2[vertices.Length];
            var triangles = new int[rayCount * 3];

            vertices[0] = origin;

            var vertexIndex = 1;
            var triangleIndex = 0;

            for (var i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                var raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);

                if (raycastHit2D.collider == null)
                    vertex = origin + GetVectorFromAngle(angle) * viewDistance;
                else vertex = raycastHit2D.point;

                vertices[vertexIndex] = vertex;

                if (i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3;
                }

                vertexIndex++;
                angle -= angleIncrease;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
        }

        public void SetOrigin(Vector3 origin) => this.origin = origin;

        private Vector3 GetVectorFromAngle(float angle)
        {
            var angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
    }
}
