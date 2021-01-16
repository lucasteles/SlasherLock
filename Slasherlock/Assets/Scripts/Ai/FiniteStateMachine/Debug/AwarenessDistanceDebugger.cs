using UnityEngine;

namespace Assets.Scripts.Ai.FiniteStateMachine.Debug
{
    public class AwarenessDistanceDebugger : MonoBehaviour
    {
        SurroundingAwareness awareness;

        void Awake()
        {
            awareness = GetComponent<SurroundingAwareness>();
        }

        private void Update()
        {
            if (awareness.LastTargetFound != null)
            {
                var distance = Vector2.Distance(awareness.LastTargetFound.position, transform.position);
                print("character distance " + distance);
            }
        }
    }
}
