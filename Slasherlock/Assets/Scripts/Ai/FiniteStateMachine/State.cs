﻿namespace Assets.Scripts.Ai.FiniteStateMachine
{
    public interface IState
    {
        void Update();
        void OnEnter();
        void OnExit();
        void Execute();
    }

    public abstract class State : IState
    {
        protected Fsm fsm;
        Transition[] transitions;

        public State(Fsm fsm)
            => this.fsm = fsm;

        public void SetTransitions(params Transition[] transitions)
            => this.transitions = transitions;

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Execute();

        public void Update()
        {
            Execute();
            CheckTransitions();
        }

        private void CheckTransitions()
        {
            if (transitions == null) return;
            foreach (var transition in transitions)
            {
                if (transition.IsValid())
                {
                    transition.OnTransition();
                    fsm.ChangeState(transition.NextState);
                }
            }
        }
    }
}