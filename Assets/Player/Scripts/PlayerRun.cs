using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRun : IPlayerBehaviour
    {
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
            Debug.Log("RUN");
        }

        public void UpdateBehaviour(PlayerStateMachine player)
        {
            if (player.playerJump.CanJump() && PlayerInputs.GetSouthButton())
            {
                player.playerJump.StartJump(player);
                return;
            }
        }

        public void FixedUpdateBehaviour(PlayerStateMachine player)
        {
            player.playerJump.CheckCollisions(player, player.playerData, player.capsuleCollider);
            
            HandleDirection(player);
            
            player.playerJump.HandleGravity(player);
            
            player.ApplyMovement();
            
            if (!player.playerJump.isGrounded)
                player.ChangeBehaviour(player.playerJump);
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
