using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRun : IPlayerBehaviour
    {
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            HandleDirection(player);
            
            player.ApplyMovement();
        }
        
        public void HandleDirection(PlayerStateMachine player)
        {
            Vector3 move = (player.moveInput.x * player.orientationPivot.right + player.moveInput.y * player.orientationPivot.forward).normalized;
            move *= player.playerData.groundMaxSpeed;
            
            if (player.moveInput.magnitude <= 0.05f)
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, 0.0f, player.playerData.groundDeceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, 0.0f, player.playerData.groundDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, move.x, player.playerData.groundAcceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, move.z, player.playerData.groundAcceleration * Time.fixedDeltaTime);
            }
        }

        public void StopBehaviour(PlayerStateMachine player, BehaviourType next)
        {
        }

        public BehaviourType GetBehaviourType()
        {
            return BehaviourType.Run;
        }
    }
}
