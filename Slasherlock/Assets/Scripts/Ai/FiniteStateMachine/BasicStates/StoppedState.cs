namespace Assets.Scripts.Ai.FiniteStateMachine.BasicStates
{
    public class StoppedState : State
    {
        public StoppedState(Fsm fsm) : base(fsm) { }

        public override void Execute() { }

        public override void OnEnter() => fsm.Mover.StopInput();

        public override void OnExit() { }

        public override string ToString()
            => typeof(StoppedState).Name;
    }
}
