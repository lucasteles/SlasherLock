namespace Assets.Scripts.Ai.FiniteStateMachine.BasicStates
{
    public class FollowingTarget : State
    {
        public FollowingTarget(Fsm fsm) : base(fsm) { }

        public override void Execute() { } 

        public override void OnEnter()
            => fsm.PathFinder.FollowTarget(fsm.Awareness.LastTargetFound);

        public override void OnExit() => fsm.PathFinder.StopFollowing();

        public override string ToString() 
            => typeof(FollowingTarget).Name;
    }
}
