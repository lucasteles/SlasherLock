using System;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class Mover : MonoBehaviour
    {
        Vector2 currentVelocity;

        float xInput;
        float yInput;

        [SerializeField] float moveSpeed;
        [SerializeField] LayerMask collisionMask;

        Rigidbody2D rb;
        
        public void SetXInput(float xInput) => this.xInput = xInput;
        public void SetYInput(float yInput) => this.yInput = yInput;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            currentVelocity.x = xInput * moveSpeed;
            currentVelocity.y = yInput * moveSpeed;

            var velocity = currentVelocity * Time.fixedDeltaTime;

            //transform.Translate(velocity);
            rb.MovePosition(rb.position + velocity);
        }
    }
}
