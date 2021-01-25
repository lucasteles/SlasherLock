﻿using Assets.Scripts.Ui.Character;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetOnSightTransition : Transition
    {
        readonly AudioClip seeYou;
        float lastTimeSee;
        float minTimeToSeeAgain = .5f;

        public TargetOnSightTransition(Fsm fsm, IState nextState, AudioClip seeYou) : base(fsm, nextState)
        {
            this.seeYou = seeYou;
        }

        public override bool IsValid()
        {
            lastTimeSee += Time.deltaTime;
            return fsm.Awareness.HasTargetOnSight();
        }

        public override void OnTransition()
        {
            if (lastTimeSee >= minTimeToSeeAgain)
            {
                var audio = fsm.gameObject.GetComponent<AudioSource>();
                audio.PlayOneShot(seeYou);
                SawEnemyThought.Instance.PlaySawEnemyThought();
                lastTimeSee = 0;
            }
        }
    }
}
