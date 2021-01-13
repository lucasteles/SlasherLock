using Assets.Scripts.Ai.Awareness;
using Assets.Scripts.Ai.PathFinding;
using Assets.Scripts.Physics;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine
{
    public abstract class Fsm : MonoBehaviour
    {
        protected IState currentState;
        public Mover Mover { private set; get; }
        public PathFinder PathFinder { private set; get; }
        public SurroundingAwareness Awareness { private set; get; }

        protected abstract void SetupStates();

        private void Awake()
        {
            SetupStates();

            Mover = GetComponent<Mover>();
            Awareness = GetComponentInChildren<SurroundingAwareness>();
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