using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class Mover : MonoBehaviour
    {
        Vector2 currentVelocity;

        float xInput;
        float yInput;

        [SerializeField] float moveSpeed;

        public void SetXInput(float xInput) => this.xInput = xInput;

        public void SetYInput(float yInput) => this.yInput = yInput;
        public void StopInput()
        {
            xInput = 0;
            yInput = 0;
        }

        void FixedUpdate()
        {
            currentVelocity.x = xInput * moveSpeed;
            currentVelocity.y = yInput * moveSpeed;

            var velocity = currentVelocity * Time.fixedDeltaTime;
            transform.Translate(velocity);
        }
    }
}
