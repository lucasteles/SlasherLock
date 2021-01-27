using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.Characters.Enemy;
using Pathfinding;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Ai.FiniteStateMachine.BasicStates
{
    public class WalkAroundState : FollowingTarget
    {
        bool debug = false;

        readonly Func<int> numberOfFlagsToLookAt;
        readonly Func<int> numberOfFlagsToSkip;
        float timeToFindNextFlag = 5;
        float elapsedTime = 0;
        WalkFlag[] flags;
        WalkFlag lastFlag;
        GameObject player;
        bool startWalking = false;
        bool waitingTeleport;
        Coroutine teleport;

        bool walkFar = false;

        public WalkAroundState(
            Fsm fsm,
            AudioClip tryingToOpenDoorSound,
            Func<float> brokeDoorPercentage,
            float timeWaitWhenWalking,
            Func<int> numberOfFlagsToLookAt,
            Func<int> numberOfFlagsToSkip
        ) : base(fsm, tryingToOpenDoorSound, brokeDoorPercentage, timeWaitWhenWalking)
        {
            this.numberOfFlagsToLookAt = numberOfFlagsToLookAt;
            this.numberOfFlagsToSkip = numberOfFlagsToSkip;
        }


        public override void UpdateState()
        {
            if (!startWalking || walkFar) return;

            if (elapsedTime > timeToFindNextFlag)
            {
                FollowFlag();
                elapsedTime = 0;
            }
            else
                elapsedTime += Time.deltaTime;
        }

        void FollowFlag()
        {
            var nearFlags = flags
                .OrderBy(x => x.distanceFromPlayer)
                .Where(NotCloseTo)
                .ToArray();

            if ((!CanReach(player.transform) && !fsm.Awareness.HasTargetOnSight())
                || Vector2.Distance(fsm.transform.position, player.transform.position) > fsm.Awareness.SightRadius * 1.5)
                Teleport(nearFlags);
            else
                StopTeleport();

            var possibleFlags =
                nearFlags
                    .Where(x => CanReach(x.transform))
                    .Take(numberOfFlagsToLookAt())
                    .ToArray();

            if (possibleFlags.Length > 0)
            {
                var selectedFlag = possibleFlags[Random.Range(0, possibleFlags.Length)];
                ChangeFlag(selectedFlag);
                fsm.PathFinder.FollowTarget(selectedFlag.gameObject.transform);
            }
        }

        void ChangeFlag(WalkFlag selectedFlag)
        {
            if (debug)
            {
                if (lastFlag) lastFlag.Hide();
                if (selectedFlag) selectedFlag.Show();
            }

            lastFlag = selectedFlag;
        }

        void Teleport(WalkFlag[] nearFlags)
        {
            if (waitingTeleport) return;
            waitingTeleport = true;
            fsm.PathFinder.StopFollowing();
            UnityEngine.Debug.Log("start teport");

            IEnumerator wait()
            {
                yield return new WaitForSeconds(1);

                UnityEngine.Debug.Log("doing teleport");
                var possibleFlags =
                    nearFlags
                        .Where(x => !x.isVisible)
                        .Where(NotCloseTo)
                        .Skip(numberOfFlagsToSkip())
                        .Take(numberOfFlagsToLookAt())
                        .ToArray();

                if (possibleFlags.Length > 0)
                {
                    var flagToTeleport = possibleFlags[Random.Range(0, possibleFlags.Length)];
                    ChangeFlag(flagToTeleport);
                    fsm.gameObject.transform.position = flagToTeleport.transform.position;
                    waitingTeleport = false;
                }
            }

            teleport = fsm.StartCoroutine(wait());
        }

        private bool NotCloseTo(WalkFlag x) => Vector2.Distance(fsm.transform.position, x.transform.position) > 5;

        public bool CanReach(Transform transform)
        {
            GraphNode getNode(Vector2 position) => AstarPath.active.GetNearest(position).node;

            return PathUtilities.IsPathPossible(
                getNode(fsm.gameObject.transform.position),
                getNode(transform.position));
        }

        public override void OnEnter()
        {
            flags = Object.FindObjectsOfType<WalkFlag>();
            player = Object.FindObjectOfType<CharacterInventary>().gameObject;

            UnityEngine.Debug.Log("Walking around...");

            IEnumerator wait()
            {
                yield return new WaitForSeconds(1);
                startWalking = true;
            }

            fsm.PathFinder.StopFollowing();
            fsm.StartCoroutine(wait());
        }

        void StopTeleport()
        {
            if (teleport == null) return;
            waitingTeleport = false;
            fsm.StopCoroutine(teleport);
        }

        public override void OnExit()
        {
            StopTeleport();
            if (lastFlag) lastFlag.Hide();
            startWalking = false;
            elapsedTime = 0;
        }

        public void WalkFarFormPlayer()
        {
            walkFar = true;

            IEnumerator walking()
            {
                yield return new WaitForSeconds(13);
                walkFar = false;
            }

            IEnumerator wait()
            {
                fsm.PathFinder.StopFollowing();
                yield return new WaitForSeconds(2);
                var flagToWalk =
                    flags
                        .OrderByDescending(x => x.distanceFromPlayer)
                        .Take(numberOfFlagsToLookAt())
                        .ToArray();

                var flagToFollow = flagToWalk[Random.Range(0, flagToWalk.Length)];
                fsm.PathFinder.FollowTarget(flagToFollow.transform);
                fsm.StartCoroutine(walking());
            }

            fsm.StartCoroutine(wait());
        }
    }
}