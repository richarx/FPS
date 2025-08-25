using UnityEngine;

namespace Player.Scripts
{
    public class ResetPlayerPosition : MonoBehaviour
    {
        [SerializeField] private float killHeight;
    
        private PlayerStateMachine player;
        private Vector3 startingPosition;
        
        private void Start()
        {
            player = PlayerStateMachine.instance;
            startingPosition = player.position + Vector3.up * 10.0f;
        }

        private void Update()
        {
            if (player.position.y <= killHeight)
                player.transform.position = startingPosition;
        }
    }
}
