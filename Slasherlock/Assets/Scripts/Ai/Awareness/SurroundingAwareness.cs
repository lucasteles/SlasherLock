using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Ai.Awareness
{
    public class SurroundingAwareness : MonoBehaviour
    {
        [SerializeField] LayerMask layersToSearchFor;
        [SerializeField] LayerMask obstacleLayer;
        [SerializeField] LayerMask playerObstacleLayer;
        [SerializeField] float sightRadius;

        public Transform LastTargetFound { private set; get; }

        public bool HasTargetOnSight()
        {
            var hit = Physics2D.CircleCast(
                transform.position, sightRadius, Vector2.one, sightRadius, layersToSearchFor);

            if (!hit) return false;

            var directionToTarget = (hit.collider.transform.position - transform.position).normalized;
            var hitFromRay = Physics2D.Raycast(transform.position, directionToTarget, sightRadius, layersToSearchFor | obstacleLayer | playerObstacleLayer);

            LastTargetFound = hit.collider.transform;
            return hit.collider == hitFromRay.collider;
        }

        [CustomEditor(typeof(SurroundingAwareness))]
        public class SurroundingAwarenessEditor : Editor
        {
            SurroundingAwareness sa;

            public void OnSceneGUI()
            {
                sa = target as SurroundingAwareness;
                if (!sa) return;

                Handles.color = Color.red;
                var transform = sa.transform;
                Handles.DrawWireDisc(transform.position, transform.forward, sa.sightRadius);
            }
        }
    }
}