namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetUnreachable : Transition
    {
        bool pathNoteExists;

        public TargetUnreachable(Fsm fsm, IState nextState) : base(fsm, nextState)
        {
            pathNoteExists = false;
        }

        public override bool IsValid() => fsm.PathFinder.IsNotPossible();

        public override void OnTransition()
        {
            fsm.PathFinder.StopFollowing();
        }

    }
}