using UnityEngine;

namespace Assets.Scripts.Fov
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask1;
        [SerializeField] LayerMask layerMask2;
        [SerializeField] LayerMask flagsLayer;
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] float fov;
        [SerializeField] float viewDistance;
        [SerializeField] int rayCount = 50;
        Mesh mesh;
        Vector3 origin;

        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void LateUpdate()
        {
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
                var vectorForCurrentAngle = GetVectorFromAngle(angle);
                var raycastHit2D =
                    Physics2D.Raycast(origin, vectorForCurrentAngle, viewDistance, layerMask1 | layerMask2);
                var flags = Physics2D.Raycast(origin, vectorForCurrentAngle, viewDistance,
                    layerMask1 | layerMask2 | flagsLayer | enemyLayer);

                if (flags.collider != null)
                {
                    MarkWalkPointAsVisible(flags.collider.gameObject);
                    MarkWalkEnemyAsVisible(flags.collider.gameObject);
                }

                if (raycastHit2D.collider == null)
                    vertex = origin + vectorForCurrentAngle * viewDistance;
                else
                {
                    var hitPoint = new Vector3(raycastHit2D.point.x, raycastHit2D.point.y);
                    vertex = hitPoint;
                }

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

        void MarkWalkPointAsVisible(GameObject gameObject)
        {
            if (gameObject.CompareTag("WalkFlag") && gameObject.GetComponent<WalkFlag>() is { } flag)
            {
                flag.SetVisibleByPlayer();
            }
        }

        void MarkWalkEnemyAsVisible(GameObject gameObject)
        {
            if (!gameObject.CompareTag("Enemy")) return;
            if (gameObject.GetComponentInChildren<ChangeLayerWheenSaw>() is { } saw)
                saw.Saw();
        }

        public void SetOrigin(Vector3 origin) => this.origin = origin;

        private Vector3 GetVectorFromAngle(float angle)
        {
            var angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
    }
}