using Assets.Scripts.Ai.FiniteStateMachine;
using Assets.Scripts.Ai.FiniteStateMachine.BasicStates;
using Assets.Scripts.Ai.FiniteStateMachine.BasicTransitions;

namespace Assets.Scripts.Characters.Enemy
{
    public class EnemyFsm : Fsm
    {
        protected override void SetupStates()
        {
            var stoppedState = new StoppedState(this);
            var followingState = new FollowingTarget(this);

            var seenTargetTransition = new TargetOnSightTransition(this, followingState);

            stoppedState.SetTransitions(seenTargetTransition);

            SetFirstState(stoppedState);
        }
    }
}
