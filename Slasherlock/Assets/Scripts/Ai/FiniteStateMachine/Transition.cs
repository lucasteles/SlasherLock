namespace Assets.Scripts.Ai.FiniteStateMachine
{
    public abstract class Transition
    {
        public IState NextState { private set; get; }

        public Transition(IState nextState)
            => NextState = nextState;

        public abstract bool IsValid();
        public abstract void OnTransition();
    }
}
