﻿using System;
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

        [SerializeField] float nextWaypointDistance = 3f;
        bool shouldMove;
        int notPossiblePathCount;

        void Awake()
        {
            seeker = GetComponent<Seeker>();
            mover = GetComponent<Mover>();
        }

        public void FollowTarget(Transform target)
        {
            notPossiblePathCount = 0;
            this.target = target;
            InvokeRepeating(nameof(UpdatePath), 0f, .5f);
        }

        public void StopFollowing()
        {
            notPossiblePathCount = 0;
            target = null;
            CancelInvoke(nameof(UpdatePath));
        }

        void UpdatePath()
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete,
                    GraphMask.FromGraphName("Grid Graph"));
            }
        }

        public bool IsNotPossible() => notPossiblePathCount > 3;

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                var targetNode = AstarPath.active.GetNearest(target.position).node;
                if (!PathUtilities.IsPathPossible(targetNode, p.path[0]))
                    notPossiblePathCount++;
                else
                    notPossiblePathCount = 0;

                path = p;
                currentWaypoint = 0;
                shouldMove = true;
            }
        }

        bool HasAnotherWayPoint() => currentWaypoint < path.vectorPath.Count;


        void FixedUpdate()
        {
            if (path == null) return;

            if (!HasAnotherWayPoint())
            {
                mover.StopInput();
                return;
            }

            var distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance || shouldMove)
            {
                currentWaypoint++;
                shouldMove = false;
                if (HasAnotherWayPoint())
                    MoveToNextWayPoint(path.vectorPath[currentWaypoint]);
            }
        }

        void MoveToNextWayPoint(Vector2 nextWaypoint)
        {
            var direction = (nextWaypoint - (Vector2) transform.position).normalized;
            mover.SetInput(direction);
        }
    }
}