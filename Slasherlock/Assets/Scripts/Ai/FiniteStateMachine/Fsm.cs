using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine
{
    public abstract class Fsm : MonoBehaviour
    {
        protected IState currentState;

        protected abstract void SetupStates();

        private void Awake()
        {
            SetupStates();
        }

        protected void SetFirstState(IState firstState)
        {
            currentState = firstState;
            currentState.OnEnter();
        }

        public void ChangeState(IState newState)
        {
            currentState.OnExit();
            newState.OnEnter();

            currentState = newState;
        }

        protected void UpdateFsm()
        {
            currentState.Update();
        }
    }
}