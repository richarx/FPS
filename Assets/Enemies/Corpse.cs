using System;
using Player.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Corpse : MonoBehaviour
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private float speed;
        [SerializeField] private GameObject explosionPrefab;

        private Vector3 directionToPlayer;
        private float distance;
        private Vector3 direction;
        private Vector3 startingPosition;
        private Vector3 velocity;

        private void Start()
        {
            startingPosition = transform.position;
            distance = Random.Range(maxDistance - 5.0f, maxDistance + 5.0f);
            ComputeDirectionToPlayer();
            ComputeMoveDirection();
        }

        private void Update()
        {
            if (Vector3.Distance(startingPosition, transform.position) >= distance)
                Explode();
            else
                Move();
        }

        private void Explode()
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void Move()
        {
            transform.position += direction * (speed * Time.deltaTime);
        }

        private void ComputeDirectionToPlayer()
        {
            Vector3 playerPosition = PlayerStateMachine.instance.transform.position;
            Vector3 position = transform.position;

            playerPosition.y = 0.0f;
            position.y = 0.0f;

            directionToPlayer = (playerPosition - position).normalized;
        }

        private void ComputeMoveDirection()
        {
            direction = (Vector3.up) - (directionToPlayer) + (Random.insideUnitSphere);
            direction.Normalize();
        }
    }
}
