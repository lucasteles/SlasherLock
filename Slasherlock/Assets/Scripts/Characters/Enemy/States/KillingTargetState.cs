using System;
using Assets.Interactables.Physics;
using Assets.Scripts.Ai.FiniteStateMachine;
using Assets.Scripts.Characters.MainCharacter;
using UnityEngine;

namespace Assets.Scripts.Characters.Enemy.States
{
    public class KillingTargetState : State
    {
        readonly AudioClip macheteSound;
        public KillingTargetState(Fsm fsm, AudioClip macheteSound) : base(fsm)
        {
            this.macheteSound = macheteSound;
        }

        public override void UpdateState() { }

        public override void OnEnter()
        {
            fsm.Mover.StopInput();

            var characterKiller = fsm.Awareness.LastTargetFound.GetComponent<MainCharacterKiller>();
            characterKiller.Kill();
            fsm.gameObject.GetComponent<AudioSource>().PlayOneShot(macheteSound);
        }

        public override void OnExit() { }

        public override string ToString() => "Killing Target State";
    }
}
