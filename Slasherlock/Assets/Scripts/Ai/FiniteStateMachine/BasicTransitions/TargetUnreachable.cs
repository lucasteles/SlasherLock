namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetUnreachable : Transition
    {
        public TargetUnreachable(Fsm fsm, IState nextState) : base(fsm, nextState) { }

        public override bool IsValid() => fsm.PathFinder.IsNotPossible();

        public override void OnTransition()
        {
            fsm.PathFinder.StopFollowing();
        }

    }
}