using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetIsClose : Transition
    {
        readonly float distanceToBeClose;

        public TargetIsClose(Fsm fsm, IState nextState, float distanceToBeClose) 
            : base(fsm, nextState)
        {
            this.distanceToBeClose = distanceToBeClose;
        }

        public override bool IsValid()
            => Vector2.Distance(fsm.Awareness.LastTargetFound.position, fsm.transform.position) < distanceToBeClose;

        public override void OnTransition() { }
    }
}
