using Player.Scripts;
using UnityEngine;

namespace Tools_and_Scripts
{
    public class LookAtPlayer : MonoBehaviour
    {
        private Transform player;
        private SpriteRenderer sr;

        private void Start()
        {
            player = PlayerStateMachine.instance.transform;
            sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;
        }

        private void LateUpdate()
        {
            transform.LookAt(player.position, Vector3.up);
        }
    }
}
