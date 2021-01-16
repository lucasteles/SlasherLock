using System;
using Assets.Scripts.Ai.FiniteStateMachine;
using Assets.Scripts.Ai.FiniteStateMachine.BasicStates;
using Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions;
using Assets.Scripts.Characters.Enemy.States;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemy
{
    public class EnemyFsm : Fsm
    {
        [SerializeField] float distanceToKillTarget;
        [SerializeField] AudioClip tryingToOpenDoorSound;
        [SerializeField] AudioClip seeYou;
        [SerializeField] AudioClip macheteSound;
        [SerializeField] float brokeDoorPercentage;
        protected override void SetupStates()
        {
            var stoppedState = new StoppedState(this);
            var followingState = new FollowingTarget(this, tryingToOpenDoorSound, brokeDoorPercentage);
            var killingTargetState = new KillingTargetState(this, macheteSound);

            var seenTargetTransition = new TargetOnSightTransition(this, followingState, seeYou);
            var targetIsCloseToKill = new TargetIsClose(this, killingTargetState, distanceToKillTarget);

            stoppedState.SetTransitions(seenTargetTransition);
            followingState.SetTransitions(targetIsCloseToKill);

            SetFirstState(stoppedState);
        }

    }
}
