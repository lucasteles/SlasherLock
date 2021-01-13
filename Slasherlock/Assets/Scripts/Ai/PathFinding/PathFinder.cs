using Assets.Scripts.Physics;
using Pathfinding;
using UnityEngine;

namespace Assets.Scripts.Ai.PathFinding
{
    [RequireComponent(typeof(Seeker))]
    [RequireComponent(typeof(Path))]
    [RequireComponent(typeof(Collider2D))]
    public class PathFinder : MonoBehaviour
    {
        Seeker seeker;
        Path path;
        Mover mover;
        Transform target;

        int currentWaypoint = 0;

        [SerializeField] float speed;
        [SerializeField] float nextWaypointDistance = 3f;


        void Awake()
        {
            seeker = GetComponent<Seeker>();
            mover = GetComponent<Mover>();
            collider = GetComponent<Collider2D>();
        }

        public void FollowTarget(Transform target) 
        { 
            this.target = target; 
            InvokeRepeating(nameof(UpdatePath), 0f, .5f);
        }

        public void StopFollowing()
        {
            target = null;
            CancelInvoke(nameof(UpdatePath));
        }

        void UpdatePath()
        {
            if (seeker.IsDone())
                seeker.StartPath(transform.position, target.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
                
                MoveToNextWayPoint(path.vectorPath[currentWaypoint]);
            }
        }

        bool HasAnotherWayPoint() => currentWaypoint < path.vectorPath.Count;

        void Update()
        {
            if (path == null) return;

            if (!HasAnotherWayPoint())
            {
                mover.StopInput();
                return;
            }

            var distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;

                if (HasAnotherWayPoint())
                    MoveToNextWayPoint(path.vectorPath[currentWaypoint]);
            }
        }

        void MoveToNextWayPoint(Vector2 nextWaypoint)
        {
            var direction = (nextWaypoint - (Vector2)transform.position).normalized;

            mover.SetXInput(direction.x);
            mover.SetYInput(direction.y);
        }
    }
}
