using System;
using Assets.Scripts.Ai.PathFinding;
using Assets.Scripts.Physics;
using Assets.Scripts.Systems.Observables;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(SurroundingAwareness))]
    public abstract class Fsm : EventSender<IState>
    {

        [SerializeField] string currentStateName;
        protected IState currentState;
        public Mover Mover { private set; get; }
        public PathFinder PathFinder { private set; get; }
        public SurroundingAwareness Awareness { private set; get; }

        protected abstract void SetupStates();

        private void Awake()
        {
            Mover = GetComponent<Mover>();
            PathFinder = GetComponent<PathFinder>();
            Awareness = GetComponent<SurroundingAwareness>();

            Listeners.AddRange(GetComponentsInChildren<IEventListener<IState>>());
        }

        private void Start()
        {
            SetupStates();
        }

        protected void SetFirstState(IState firstState)
        {
            currentState = firstState;
            currentState.OnEnter();
            NotifyListeners(currentState);
            UpdateStateName();
        }

        void UpdateStateName() => currentStateName = currentState.GetType().Name;
        public void ChangeState(IState newState)
        {
            currentState.OnExit();
            newState.OnEnter();

            currentState = newState;
            UpdateStateName();
            NotifyListeners(currentState);
        }

        protected void UpdateFsm()
        {
            currentState.Update();
        }

        void OnTriggerEnter2D(Collider2D other) => currentState.OnTriggerEnter(other);

        void OnTriggerStay2D(Collider2D other) => currentState.OnTriggerStay(other);

        void Update()
        {
            UpdateFsm();    
        }
    }
}