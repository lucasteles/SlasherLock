using UnityEngine;

namespace Assets.Scripts.Ai.Awareness
{
    public class SurroundingAwareness : MonoBehaviour
    {
        [SerializeField] LayerMask layersToSearchFor;
        [SerializeField] float sightRadius;

        public Transform LastTargetFound { private set; get; }

        public bool HasTargetOnSight()
        {
            var hit = Physics2D.CircleCast(
                transform.position, sightRadius, Vector2.one, sightRadius, layersToSearchFor);

            if (hit)
            {
                var directionToTarget = (hit.collider.transform.position - transform.position).normalized;
                var hitFromRay = Physics2D.Raycast(transform.position, directionToTarget, sightRadius);

                LastTargetFound = hit.collider.transform;
                return hit.collider == hitFromRay.collider;
            }

            return false;
        }
    }
}
