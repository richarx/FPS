using UnityEngine;
using UnityEngine.Events;

namespace Player.Scripts
{
    public class PlayerSlam : IPlayerBehaviour
    {
        public UnityEvent OnStartSlam = new UnityEvent();
        public UnityEvent OnEndSlam = new UnityEvent();
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("SLAM");
            player.moveVelocity.y = 0.0f;
            OnStartSlam?.Invoke();
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            bool landed = player.playerJump.CheckCollisions(player);
            
            player.playerJump.HandleDirection(player);
            HandleGravity(player);

            player.ApplyMovement();
            
            if (landed)
                player.ChangeBehaviour(player.playerSlide);
        }

        private void HandleGravity(PlayerStateMachine player)
        {
            player.moveVelocity.y = Mathf.MoveTowards(player.moveVelocity.y, -player.playerData.slamMaxSpeed, player.playerData.slamAcceleration * Time.fixedDeltaTime);
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Slam;
        }
    }
}
