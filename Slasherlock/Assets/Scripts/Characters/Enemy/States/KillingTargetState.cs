using Assets.Scripts.Ai.FiniteStateMachine;
using Assets.Scripts.Characters.MainCharacter;

namespace Assets.Scripts.Characters.Enemy.States
{
    public class KillingTargetState : State
    {
        public KillingTargetState(Fsm fsm) : base(fsm) { }

        public override void Execute() { }

        public override void OnEnter()
        {
            fsm.Mover.StopInput();

            var characterKiller = fsm.Awareness.LastTargetFound.GetComponent<MainCharacterKiller>();
            characterKiller.Kill();
        }

        public override void OnExit() { }

        public override string ToString() => "Killing Target State";
    }
}
