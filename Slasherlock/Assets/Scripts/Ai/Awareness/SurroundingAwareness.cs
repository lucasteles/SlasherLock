using UnityEngine;

public class SurroundingAwareness : MonoBehaviour
{
    [SerializeField] LayerMask layersToSearchFor;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] LayerMask playerObstacleLayer;
    [SerializeField] float sightRadius;
    public float SightRadius => sightRadius;

    public Transform LastTargetFound { private set; get; }

    public bool HasTargetOnSight()
    {
        var hit = Physics2D.CircleCast(
            transform.position, sightRadius, Vector2.one, sightRadius, layersToSearchFor);

        if (!hit) return false;

        var directionToTarget = (hit.collider.transform.position - transform.position).normalized;
        var hitFromRay = Physics2D.Raycast(transform.position, directionToTarget, sightRadius,
            layersToSearchFor | obstacleLayer | playerObstacleLayer);

        Debug.DrawLine(transform.position, hitFromRay.point, hit.collider == hitFromRay.collider ? Color.magenta : Color.yellow);
        LastTargetFound = hit.collider.transform;
        return hit.collider == hitFromRay.collider;
    }

    public bool CanReachLastTarget()
    {
        var directionToTarget = (LastTargetFound.transform.position - transform.position).normalized;
        var hitFromRay = Physics2D.Raycast(transform.position, directionToTarget, sightRadius,
            layersToSearchFor | obstacleLayer | playerObstacleLayer);

        return (1 << hitFromRay.collider.gameObject.layer & layersToSearchFor) != 0 ;
    }
}