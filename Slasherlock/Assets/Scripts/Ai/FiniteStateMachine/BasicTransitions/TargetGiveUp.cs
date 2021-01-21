using System;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions
{
    public class TargetGiveUp : Transition
    {
        Func<float> timeToGiveUp;
        float elapsetTime;

        public TargetGiveUp(Fsm fsm, IState nextState, Func<float> timeToGiveUp) : base(fsm, nextState)
        {
            this.timeToGiveUp = timeToGiveUp;
        }

        public override bool IsValid()
        {
            if (fsm.Awareness.HasTargetOnSight())
                elapsetTime = 0;
            else
                elapsetTime += Time.deltaTime;

            return elapsetTime >= timeToGiveUp();
        }

        public override void OnTransition()
        {
            elapsetTime = 0;
        }
    }
}