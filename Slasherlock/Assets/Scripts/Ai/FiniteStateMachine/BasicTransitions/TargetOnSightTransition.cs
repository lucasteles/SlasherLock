namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetOnSightTransition : Transition
    {
        public TargetOnSightTransition(Fsm fsm, IState nextState) : base(fsm, nextState) {}

        public override bool IsValid() => fsm.Awareness.HasTargetOnSight();

        public override void OnTransition() {}
    }
}
