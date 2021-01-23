using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine
{
    public interface IState
    {
        void Update();
        void OnEnter();
        void OnExit();
        void OnTriggerEnter(Collider2D other);
        void UpdateState();
        void OnTriggerStay(Collider2D other);
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

        public virtual void OnTriggerStay(Collider2D other)
        {
        }

        public virtual void OnTriggerEnter(Collider2D other)
        {
        }

        public abstract void UpdateState();

        public override string ToString() => GetType().Name;

        public void Update()
        {
            UpdateState();
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
                    break;
                }
            }
        }
    }
}