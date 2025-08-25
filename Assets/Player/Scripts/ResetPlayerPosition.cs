using UnityEngine;

namespace Player.Scripts
{
    public class ResetPlayerPosition : MonoBehaviour
    {
        private PlayerStateMachine player;
        private Vector3 startingPosition;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            startingPosition = player.position + Vector3.up * 10.0f;
        }

        private void Update()
        {
            if (player.position.y <= -200.0f)
                player.transform.position = startingPosition;
        }
    }
}
