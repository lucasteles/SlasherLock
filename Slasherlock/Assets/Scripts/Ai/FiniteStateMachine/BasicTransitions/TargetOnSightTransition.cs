using Assets.Scripts.Ui.Character;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetOnSightTransition : Transition
    {
        readonly AudioClip seeYou;

        public TargetOnSightTransition(Fsm fsm, IState nextState, AudioClip seeYou) : base(fsm, nextState)
        {
            this.seeYou = seeYou;
        }

        public TargetOnSightTransition(Fsm fsm, IState nextState) : base(fsm, nextState) {}

        public override bool IsValid() => fsm.Awareness.HasTargetOnSight();

        public override void OnTransition()
        {
            var audio = fsm.gameObject.GetComponent<AudioSource>();
            audio.PlayOneShot(seeYou);
            SawEnemyThought.Instance.PlaySawEnemyThought();
        }
    }
}
