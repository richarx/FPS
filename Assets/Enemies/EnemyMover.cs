using System;
using Player.Scripts;
using UnityEngine;

namespace Enemies
{
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveAcceleration;
        [SerializeField] private float moveDeceleration;
        [SerializeField] private float hoverHeight;
        [SerializeField] private float hoverVariationMultiplier;
        [SerializeField] private float hoverVariationSpeed;
        [SerializeField] private float hoverSmoothSpeed;
        [SerializeField] private LayerMask ignoreForGroundCollision;
        
        private Transform player;
        private Rigidbody rb;
        
        private Vector3 currentDirection;
        private Vector3 currentVelocity;

        public bool IsMoving => currentVelocity.magnitude >= 0.01f;
        
        
        private float gravityVelocity;
        private Vector3 moveVelocity;

        private void Start()
        {
            player = PlayerStateMachine.instance.transform;
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            HandleGravity();
            HandleDirection();
            
            rb.velocity = currentVelocity;
        }

        private void HandleDirection()
        {
            Vector3 direction = currentDirection.normalized;
            direction.y = 0.0f;

            if (currentDirection.magnitude >= 0.15f)
                currentVelocity = Vector3.SmoothDamp(currentVelocity, direction * moveSpeed, ref moveVelocity, moveAcceleration * Time.fixedDeltaTime);
            else
                currentVelocity = Vector3.SmoothDamp(currentVelocity, Vector3.zero, ref moveVelocity, moveDeceleration * Time.fixedDeltaTime);
        }

        private void HandleGravity()
        {
            Vector3 position = transform.position;
            
            float groundHeight = ComputeGroundHeight();
            float currentHeight = position.y;
            float targetHeight = groundHeight + hoverHeight + (Mathf.Sin(Time.time * hoverVariationSpeed) * hoverVariationMultiplier);

            if (Mathf.Abs(targetHeight - currentHeight) >= 0.01f)
            {
                position.y = Mathf.SmoothDamp(currentHeight, targetHeight, ref gravityVelocity, hoverSmoothSpeed);
                rb.MovePosition(position);
            }
        }

        private float ComputeGroundHeight()
        {
            Vector3 position = transform.position + (Vector3.up * hoverHeight);
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(position, Vector3.down, out hit, hoverHeight * 3.0f, ~ignoreForGroundCollision);

            if (hasHit)
                return position.y - hit.distance;
            else
                return hoverHeight * 2.0f;
        }

        public void MoveInDirection(Vector3 direction)
        {
            currentDirection = direction.normalized;
        }

        public void StopMoving()
        {
            currentDirection = Vector3.zero;
        }
        
        public float DistanceToPlayer()
        {
            return (player.position - transform.position).magnitude;
        }
        
        public Vector3 DirectionToPlayer()
        {
            return (player.position - transform.position).normalized;
        }
        
        public  Vector3 DirectionToPlayerFlatten()
        {
            Vector3 direction = DirectionToPlayer();
            direction.y = 0.0f;
            return direction.normalized;
        }
    }
}
