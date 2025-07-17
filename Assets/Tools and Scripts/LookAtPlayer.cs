using Player.Scripts;
using UnityEngine;

namespace Tools_and_Scripts
{
    public class LookAtPlayer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr;
        
        private Transform player;

        private void Start()
        {
            player = PlayerStateMachine.instance.transform;
            
            if (sr == null)
                sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;
        }

        private void LateUpdate()
        {
            transform.LookAt(player.position, Vector3.up);
        }
    }
}
