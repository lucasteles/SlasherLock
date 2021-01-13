namespace Assets.Scripts.Ai.FiniteStateMachine
{
    public abstract class Transition
    {
        protected readonly Fsm fsm;
        public IState NextState { private set; get; }

        public Transition(Fsm fsm, IState nextState)
        {
            this.fsm = fsm;
            NextState = nextState;
        }

        public abstract bool IsValid();
        public abstract void OnTransition();
    }
}
