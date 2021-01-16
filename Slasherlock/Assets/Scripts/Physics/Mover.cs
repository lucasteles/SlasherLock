using UnityEngine;

namespace Assets.Scripts.Physics
{
    public enum WalkSide
    {
        Left,
        Right,
        Up,
        Down
    }

    public class Mover : MonoBehaviour
    {
        Vector2 currentVelocity;
        bool canMove = true;
        float xInput;
        float yInput;

        [SerializeField] float moveSpeed;
        Rigidbody2D rb;
        public WalkSide Side { get; private set; } = WalkSide.Down;

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

            UpdateSide();
            var velocity = currentVelocity * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + velocity);
        }

        void UpdateSide()
        {
            var diff = currentVelocity.normalized;
            Side = (diff.x, diff.y) switch
            {
                (-1, 0) => WalkSide.Left,
                (1, 0) => WalkSide.Right,
                (0, 1) => WalkSide.Up,
                (0, -1) => WalkSide.Down,
                _ => Side
            };
        }
    }
}