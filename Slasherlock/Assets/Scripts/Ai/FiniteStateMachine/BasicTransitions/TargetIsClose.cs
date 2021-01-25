using System;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetIsClose : Transition
    {
        readonly float distanceToBeClose;
        readonly Func<bool> canKill;

        public TargetIsClose(Fsm fsm, IState nextState, float distanceToBeClose, Func<bool> canKill)
            : base(fsm, nextState)
        {
            this.distanceToBeClose = distanceToBeClose;
            this.canKill = canKill;
        }

        public override bool IsValid()
        {
            var distance = Vector2.Distance(fsm.Awareness.LastTargetFound.position, fsm.transform.position) <
                           distanceToBeClose;

            return distance && canKill() && fsm.Awareness.CanReachLastTarget();
        }

        public override void OnTransition()
        {
        }
    }
}