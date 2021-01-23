using System;
using System.Linq;
using Assets.Scripts.Ai.FiniteStateMachine;
using Assets.Scripts.Ai.FiniteStateMachine.BasicStates;
using Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions;
using Assets.Scripts.Characters.Enemy.States;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Characters.Enemy
{
    public class EnemyFsm : Fsm
    {
        [SerializeField] float distanceToKillTarget;
        [SerializeField] AudioClip tryingToOpenDoorSound;
        [SerializeField] AudioClip seeYou;
        [SerializeField] AudioClip macheteSound;
        [SerializeField] float brokeDoorPercentage;
        [SerializeField] float timeToGiveUp;
        [SerializeField] float timeToWaitWhenWalkingAndSeePlayer;
        [SerializeField] int numberOfFlagsToLookAt;

        State[] allStates;
        public void SetBrokeDoorPercentage(float v) => brokeDoorPercentage = v;
        public void SetTimeToGiveUp(float v) => timeToGiveUp = v;
        protected override void SetupStates()
        {
            var stoppedState = new StoppedState(this);
            var followingState = new FollowingTarget(this, tryingToOpenDoorSound,
                () => brokeDoorPercentage, timeToWaitWhenWalkingAndSeePlayer);
            var walkAroundState = new WalkAroundState(this, tryingToOpenDoorSound,
                () => brokeDoorPercentage,timeToWaitWhenWalkingAndSeePlayer, () => numberOfFlagsToLookAt);
            var killingTargetState = new KillingTargetState(this, macheteSound);

            var seenTargetTransition = new TargetOnSightTransition(this, followingState, seeYou);
            var targetIsCloseToKill = new TargetIsClose(this, killingTargetState, distanceToKillTarget);
            var targetUnreachable = new TargetUnreachable(this, walkAroundState);
            var targetGiveUp = new TargetGiveUp(this, walkAroundState, () => timeToGiveUp);

            //TODO: voltar killing state
            stoppedState.SetTransitions(seenTargetTransition);
            followingState.SetTransitions(
                // targetIsCloseToKill,
                targetUnreachable,
                targetGiveUp);
            walkAroundState.SetTransitions(seenTargetTransition);
            SetFirstState(stoppedState);

            allStates = new State[] {stoppedState, followingState, walkAroundState, killingTargetState};
        }


        public void MoveAndSetState<T>(Vector2 position) where T : State
        {
            transform.position = position;
            SetFirstState(allStates.OfType<T>().FirstOrDefault());
        }
    }
}
