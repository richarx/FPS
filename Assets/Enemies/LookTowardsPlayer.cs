using System;
using Player.Scripts;
using UnityEngine;

namespace Enemies
{
    public class LookTowardsPlayer : MonoBehaviour
    {
        [SerializeField] private Transform orientation;
        
        private Transform player;
    
        private void Start()
        {
            player = PlayerStateMachine.instance.transform;
        }

        private void Update()
        {
            float angle = Vector3.SignedAngle(transform.forward, DirectionToPlayerFlatten(), Vector3.up);
            orientation.rotation = Quaternion.Euler(0, angle, 0);
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

        // private void OnDrawGizmos()
        // {
        //     Vector3 position = transform.position;
        //     
        //     Gizmos.color = Color.blue;
        //     Gizmos.DrawLine(position, position + (transform.forward * 2.0f));
        //     
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawLine(position, position + (DirectionToPlayerFlatten() * 4.0f));
        //     
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawLine(position, position + (orientation.forward * 8.0f));
        // }
    }
}
