using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class Mover : MonoBehaviour
    {
        Vector2 currentVelocity;
        bool canMove = true;
        float xInput;
        float yInput;

        [SerializeField] float moveSpeed;
        Rigidbody2D rb;

        public void AllowMovement() => canMove = true;
        public void PreventMovement()
        {
            StopInput();
            canMove = false;
        }

        public void SetXInput(float xInput) => this.xInput = xInput;
        public void SetYInput(float yInput) => this.yInput = yInput;

        public void StopInput()
        {
            xInput = 0;
            yInput = 0;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (!canMove) return;

            currentVelocity.x = xInput * moveSpeed;
            currentVelocity.y = yInput * moveSpeed;

            var velocity = currentVelocity * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + velocity);
        }
    }
}
