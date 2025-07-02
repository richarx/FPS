using Tools_and_Scripts;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerRun : IPlayerBehaviour
    {
        private RaycastHit slopeHit;
        
        public void StartBehaviour(PlayerStateMachine player, BehaviourType previous)
        {
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

        private bool CheckIsOnSlope(PlayerStateMachine player)
        {
            if (Physics.Raycast(player.position + (Vector3.up * 0.1f), Vector3.down, out slopeHit, 0.3f, ~player.playerData.layersToIgnoreForGroundCheck))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < player.playerData.maxSlopeAngle && angle != 0.0f;
            }

            return false;
        }

        private Vector3 ComputeSlopeMoveDirection(Vector3 moveDirection)
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }

        public void HandleDirection(PlayerStateMachine player)
        {
            Vector3 move = (player.moveInput.x * player.orientationPivot.right + player.moveInput.y * player.orientationPivot.forward).normalized;
            move *= player.isAiming ? player.playerData.groundMaxSpeedAiming : player.playerData.groundMaxSpeed;

            bool isOnSLope = CheckIsOnSlope(player);
            
            if (player.moveInput.magnitude <= 0.05f)
            {
                player.moveVelocity.x = Mathf.MoveTowards(player.moveVelocity.x, 0.0f, player.playerData.groundDeceleration * Time.fixedDeltaTime);
                player.moveVelocity.z = Mathf.MoveTowards(player.moveVelocity.z, 0.0f, player.playerData.groundDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                if (isOnSLope)
                    move = ComputeSlopeMoveDirection(move);       
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
