using UnityEngine;

namespace Assets.Physics
{
    public class Mover : MonoBehaviour
    {
        Vector2 currentVelocity;

        float xInput;
        float yInput;

        [SerializeField] float moveSpeed;
        [SerializeField] LayerMask collisionMask;

        public void SetXInput(float xInput) => this.xInput = xInput;
        public void SetYInput(float yInput) => this.yInput = yInput;

        void FixedUpdate()
        {
            currentVelocity.x = xInput * moveSpeed;
            currentVelocity.y = yInput * moveSpeed;

            var velocity = currentVelocity * Time.fixedDeltaTime;

            transform.Translate(velocity);
        }
    }
}
