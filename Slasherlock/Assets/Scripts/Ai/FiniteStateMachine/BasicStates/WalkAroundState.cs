using System;
using System.Collections;
using System.Linq;
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
        float timeToFindNextFlag = 5;
        float elapsedTime = 0;
        WalkFlag[] flags;
        WalkFlag lastFlag;
        GameObject player;
        bool startWalking = false;
        bool waitingTeleport;
        Coroutine teleport;

        public WalkAroundState(
            Fsm fsm,
            AudioClip tryingToOpenDoorSound,
            Func<float> brokeDoorPercentage,
            float timeWaitWhenWalking,
            Func<int> numberOfFlagsToLookAt
        ) : base(fsm, tryingToOpenDoorSound, brokeDoorPercentage, timeWaitWhenWalking)
        {
            this.numberOfFlagsToLookAt = numberOfFlagsToLookAt;
        }


        public override void UpdateState()
        {
            if (!startWalking) return;

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

            if (!CanReach(player.transform)
                || Vector2.Distance(fsm.transform.position, player.transform.position) > fsm.Awareness.SightRadius * 2)
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
                yield return new WaitForSeconds(3);

                UnityEngine.Debug.Log("doing teleport");
                var possibleFlags =
                    nearFlags
                        .Where(x => !x.isVisible)
                        .Where(NotCloseTo)
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
            fsm.StopCoroutine(teleport);
            waitingTeleport = false;
        }

        public override void OnExit()
        {
            if (lastFlag) lastFlag.Hide();
            startWalking = false;
            elapsedTime = 0;
        }
    }
}