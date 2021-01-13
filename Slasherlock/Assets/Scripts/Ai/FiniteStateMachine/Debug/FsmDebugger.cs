using Assets.Scripts.Systems.Observables;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine.Debug
{
    public class FsmDebugger : MonoBehaviour, IEventListener<IState>
    {
        [SerializeField] TextMeshProUGUI currentState;

        public void Notify(IState e)
        {
            currentState.text = e.ToString();
        }
    }
}
