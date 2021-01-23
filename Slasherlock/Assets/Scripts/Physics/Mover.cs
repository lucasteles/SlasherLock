using System;
using System.Collections;
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
        [SerializeField] float waitFor;
        Rigidbody2D rb;
        public WalkSide Side { get; private set; } = WalkSide.Down;

        public void AllowMovement() => canMove = true;

        public void PreventMovement()
        {
            StopInput();
            canMove = false;
        }
        
        public void Move(Vector2 position) => rb.MovePosition(position);

        public void SetMoveSeed(float v) => moveSpeed = v;
        public void SetXInput(float xInput) => this.xInput = xInput;
        public void SetYInput(float yInput) => this.yInput = yInput;

        public void SetInput(Vector2 mv) => (xInput, yInput) = (mv.x, mv.y);


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
            var velocity = currentVelocity * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + velocity);
        }

        void Update()
        {
            if (!canMove) return;
            currentVelocity.x = xInput * moveSpeed;
            currentVelocity.y = yInput * moveSpeed;
            UpdateSide();
        }

        void UpdateSide()
        {
            // print($"{xInput}|{yInput}");
            var leftOrRight =
                xInput > 0f ? 1
                : xInput < 0f ? -1
                : 0;

            var upOrDown =
                yInput > 0f ? 1
                : yInput < 0f ? -1
                : 0;

            Side = (leftOrRight, upOrDown) switch
            {
                (-1, 0) => WalkSide.Left,
                (1, 0) => WalkSide.Right,
                (0, 1) => WalkSide.Up,
                (0, -1) => WalkSide.Down,

                (-1, 1) when Mathf.Abs(xInput) > Mathf.Abs(yInput) => WalkSide.Left,
                (-1, -1) when Mathf.Abs(xInput) > Mathf.Abs(yInput) => WalkSide.Left,
                (1, 1) when Mathf.Abs(xInput) > Mathf.Abs(yInput) => WalkSide.Right,
                (1, -1) when Mathf.Abs(xInput) > Mathf.Abs(yInput) => WalkSide.Right,

                (-1, 1) => WalkSide.Up,
                (1, 1) => WalkSide.Up,
                (-1, -1) => WalkSide.Down,
                (1, -1) => WalkSide.Down,
                _ => Side
            };
        }

        public void Wait()
        {
            PreventMovement();

            IEnumerator wait()
            {
                yield return new WaitForSeconds(waitFor);
                AllowMovement();
            }

            StartCoroutine(wait());
        }
    }
}