using Player.Scripts;
using UnityEngine;

namespace Enemies
{
    public class ChasePlayer : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float aggroRange;
        [SerializeField] private float stopMovingRange;
            
        private Transform player;
        private Rigidbody rb;

        private Vector3 currentVelocity;
        
        private void Start()
        {
            player = PlayerStateMachine.instance.transform;
            rb = GetComponent<Rigidbody>();
        }

        private void LateUpdate()
        {
            float distance = DistanceToPlayer();

            if (distance >= aggroRange || distance <= stopMovingRange)
                StopMoving();
            else
                MoveTowardsPlayer();
        }

        private void FixedUpdate()
        {
            rb.velocity = currentVelocity;
        }

        private void MoveTowardsPlayer()
        {
            Vector3 direction = DirectionToPlayerFlatten();
            currentVelocity = direction * moveSpeed;
        }

        private void StopMoving()
        {
            currentVelocity = Vector3.zero;
        }

        private float DistanceToPlayer()
        {
            return (player.position - transform.position).magnitude;
        }
        
        private Vector3 DirectionToPlayer()
        {
            return (player.position - transform.position).normalized;
        }
        
        private Vector3 DirectionToPlayerFlatten()
        {
            Vector3 direction = DirectionToPlayer();
            direction.y = 0.0f;
            return direction.normalized;
        }
    }
}
