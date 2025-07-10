using Player.Scripts;
using UnityEngine;

namespace Tools_and_Scripts
{
    public class LookAtPlayerWithRotation : MonoBehaviour
    {
        private Transform player;
        private SpriteRenderer sr;
        private Vector3 randomDirection;

        private void Start()
        {
            player = PlayerStateMachine.instance.transform;
            sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;

            randomDirection = Random.insideUnitSphere;
        }

        private void LateUpdate()
        {
            transform.LookAt(player.position, Vector3.up + randomDirection);
        }
    }
}
